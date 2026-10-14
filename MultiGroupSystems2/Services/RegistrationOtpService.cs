using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using MultiGroupSystemsTester.Services;

namespace MultiGroupSystemsTester
{
    /// <summary>
    /// SMS codes that prove a new customer owns the phone number they typed in, before the account is created.
    /// Same idea as OtpService, but keyed on a per-visit token instead of a CustomerID because no customer exists yet.
    /// Reuses the OtpSendResult / OtpVerifyResult enums defined in OtpService.cs.
    /// </summary>
    public static class RegistrationOtpService
    {
        private const int ExpiryMinutes = 5;
        private const int MaxAttempts = 3;
        private const int ResendCooldownSeconds = 60;
        private const int MaxSendsPerPhonePerHour = 5;
        private const int MaxSendsPerIpPerHour = 10;

        private static string ConnStr
        {
            get { return ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString; }
        }

        public static string NewToken()
        {
            return RandomHex(32);
        }

       
        public static OtpSendResult CreateAndSend(string token, string phoneNumber, string ip, bool enforceCooldown, out string debugError)
        {
            debugError = null;
            if (string.IsNullOrWhiteSpace(phoneNumber)) return OtpSendResult.NoPhone;
            if (ip != null && ip.Length > 45) ip = ip.Substring(0, 45);

            string code = RandomCode();

            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                using (var cleanup = new SqlCommand(
                    "DELETE FROM RegistrationOtp WHERE ExpiresAt < DATEADD(DAY, -1, SYSUTCDATETIME())", conn))
                {
                    cleanup.ExecuteNonQuery();
                }

                int perPhone;
                int perIp;
                int secondsSinceLast;
                using (var cmd = new SqlCommand(
                    @"SELECT
                        (SELECT COUNT(*) FROM RegistrationOtp
                         WHERE PhoneNumber = @Phone AND CreatedAt > DATEADD(HOUR, -1, SYSUTCDATETIME())) AS PerPhone,
                        (SELECT COUNT(*) FROM RegistrationOtp
                         WHERE RequestIP = @IP AND CreatedAt > DATEADD(HOUR, -1, SYSUTCDATETIME())) AS PerIp,
                        (SELECT ISNULL(MIN(DATEDIFF(SECOND, CreatedAt, SYSUTCDATETIME())), 999999)
                         FROM RegistrationOtp WHERE Token = @Token) AS SecondsSinceLast", conn))
                {
                    cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                    cmd.Parameters.AddWithValue("@IP", (object)ip ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Token", token);
                    using (var r = cmd.ExecuteReader())
                    {
                        r.Read();
                        perPhone = Convert.ToInt32(r["PerPhone"]);
                        perIp = Convert.ToInt32(r["PerIp"]);
                        secondsSinceLast = Convert.ToInt32(r["SecondsSinceLast"]);
                    }
                }

                if (perPhone >= MaxSendsPerPhonePerHour || perIp >= MaxSendsPerIpPerHour) return OtpSendResult.TooManyRequests;
                if (enforceCooldown && secondsSinceLast < ResendCooldownSeconds) return OtpSendResult.TooSoon;

                
                using (var cmd = new SqlCommand(
                    "UPDATE RegistrationOtp SET UsedAt = SYSUTCDATETIME() WHERE Token = @Token AND UsedAt IS NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.ExecuteNonQuery();
                }

                int otpId;
                using (var cmd = new SqlCommand(
                    @"INSERT INTO RegistrationOtp (Token, PhoneNumber, CodeHash, RequestIP, ExpiresAt)
                      OUTPUT inserted.OtpID
                      VALUES (@Token, @Phone, @Hash, @IP, DATEADD(MINUTE, @Mins, SYSUTCDATETIME()))", conn))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                    cmd.Parameters.AddWithValue("@Hash", HashCode(token, code));
                    cmd.Parameters.AddWithValue("@IP", (object)ip ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mins", ExpiryMinutes);
                    otpId = Convert.ToInt32(cmd.ExecuteScalar());
                }

              
                System.Diagnostics.Debug.WriteLine("REGISTRATION OTP for " + phoneNumber + ": " + code);

                try
                {
                    string message = "Your Multigroup Inc. verification code is " + code +
                                     ". It expires in " + ExpiryMinutes + " minutes. Never share this code with anyone.";

                   
                    SmsService.SendSms(phoneNumber, message);
                }
                catch (Exception ex)
                {
                    debugError = ex.Message;
                    System.Diagnostics.Debug.WriteLine("Registration OTP SMS failed: " + ex.Message);

                    using (var cmd = new SqlCommand(
                        "UPDATE RegistrationOtp SET UsedAt = SYSUTCDATETIME() WHERE OtpID = @OtpId", conn))
                    {
                        cmd.Parameters.AddWithValue("@OtpId", otpId);
                        cmd.ExecuteNonQuery();
                    }
                    return OtpSendResult.SendFailed;
                }
            }

            return OtpSendResult.Sent;
        }

       
        public static OtpVerifyResult Verify(string token, string enteredCode)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                int otpId;
                string storedHash;
                int failed;
                using (var cmd = new SqlCommand(
                    @"SELECT TOP 1 OtpID, CodeHash, FailedAttempts
                      FROM RegistrationOtp
                      WHERE Token = @Token AND UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()
                      ORDER BY OtpID DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return OtpVerifyResult.ExpiredOrMissing;
                        otpId = Convert.ToInt32(r["OtpID"]);
                        storedHash = r["CodeHash"].ToString().Trim();
                        failed = Convert.ToInt32(r["FailedAttempts"]);
                    }
                }

                if (failed >= MaxAttempts) return OtpVerifyResult.Locked;

                if (HashCode(token, enteredCode) != storedHash)
                {
                    bool lockNow = failed + 1 >= MaxAttempts;
                    using (var cmd = new SqlCommand(
                        @"UPDATE RegistrationOtp
                          SET FailedAttempts = FailedAttempts + 1,
                              UsedAt = CASE WHEN @Lock = 1 THEN SYSUTCDATETIME() ELSE UsedAt END
                          WHERE OtpID = @OtpId", conn))
                    {
                        cmd.Parameters.AddWithValue("@Lock", lockNow ? 1 : 0);
                        cmd.Parameters.AddWithValue("@OtpId", otpId);
                        cmd.ExecuteNonQuery();
                    }
                    return lockNow ? OtpVerifyResult.Locked : OtpVerifyResult.Wrong;
                }

                using (var cmd = new SqlCommand(
                    "UPDATE RegistrationOtp SET UsedAt = SYSUTCDATETIME() WHERE OtpID = @OtpId AND UsedAt IS NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@OtpId", otpId);
                    return cmd.ExecuteNonQuery() == 1 ? OtpVerifyResult.Success : OtpVerifyResult.ExpiredOrMissing;
                }
            }
        }

        private static string RandomCode()
        {
            var buf = new byte[4];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(buf);
            return (BitConverter.ToUInt32(buf, 0) % 1000000).ToString("D6");
        }

        private static string RandomHex(int bytes)
        {
            var buf = new byte[bytes];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(buf);
            return ToHex(buf);
        }

        private static string HashCode(string token, string code)
        {
            using (var sha = SHA256.Create())
            {
                return ToHex(sha.ComputeHash(Encoding.UTF8.GetBytes(token + ":" + code)));
            }
        }

        private static string ToHex(byte[] data)
        {
            var sb = new StringBuilder(data.Length * 2);
            foreach (byte b in data) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}