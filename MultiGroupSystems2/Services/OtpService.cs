using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MultiGroupSystemsTester.Services;

namespace MultiGroupSystemsTester
{
    public enum OtpSendResult { Sent, NoPhone, TooSoon, TooManyRequests, SendFailed }
    public enum OtpVerifyResult { Success, Wrong, Locked, ExpiredOrMissing }

   
    public static class OtpService
    {
        private const int ExpiryMinutes = 5;
        private const int MaxAttempts = 3;
        private const int ResendCooldownSeconds = 60;
        private const int MaxSendsPerHour = 5;

        private static string ConnStr
        {
            get { return ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString; }
        }

       
        public static OtpSendResult CreateAndSend(int customerId, string phoneNumber, bool enforceCooldown, out string debugError)
        {
            debugError = null;
            if (string.IsNullOrWhiteSpace(phoneNumber)) return OtpSendResult.NoPhone;

            string code = RandomCode();

            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                using (var cleanup = new SqlCommand(
                    "DELETE FROM LoginOtp WHERE ExpiresAt < DATEADD(DAY, -1, SYSUTCDATETIME())", conn))
                {
                    cleanup.ExecuteNonQuery();
                }

               
                int sentLastHour;
                int secondsSinceLast;
                using (var cmd = new SqlCommand(
                    @"SELECT COUNT(*) AS SentCount,
                             ISNULL(MIN(DATEDIFF(SECOND, CreatedAt, SYSUTCDATETIME())), 999999) AS SecondsSinceLast
                      FROM LoginOtp
                      WHERE CustomerID = @Id AND CreatedAt > DATEADD(HOUR, -1, SYSUTCDATETIME())", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    using (var r = cmd.ExecuteReader())
                    {
                        r.Read();
                        sentLastHour = Convert.ToInt32(r["SentCount"]);
                        secondsSinceLast = Convert.ToInt32(r["SecondsSinceLast"]);
                    }
                }

                if (sentLastHour >= MaxSendsPerHour) return OtpSendResult.TooManyRequests;
                if (enforceCooldown && secondsSinceLast < ResendCooldownSeconds) return OtpSendResult.TooSoon;

                
                using (var cmd = new SqlCommand(
                    "UPDATE LoginOtp SET UsedAt = SYSUTCDATETIME() WHERE CustomerID = @Id AND UsedAt IS NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    cmd.ExecuteNonQuery();
                }

                int otpId;
                using (var cmd = new SqlCommand(
                    @"INSERT INTO LoginOtp (CustomerID, CodeHash, ExpiresAt)
                      OUTPUT inserted.OtpID
                      VALUES (@Id, @Hash, DATEADD(MINUTE, @Mins, SYSUTCDATETIME()))", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    cmd.Parameters.AddWithValue("@Hash", HashCode(customerId, code));
                    cmd.Parameters.AddWithValue("@Mins", ExpiryMinutes);
                    otpId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                System.Diagnostics.Debug.WriteLine("VERIFICATION OTP for customer " + customerId + ": " + code);

                try
                {
                    string message = "Your Multigroup Inc. verification code is " + code +
                                     ". It expires in " + ExpiryMinutes + " minutes. Never share this code with anyone.";

                    SmsService.SendSms(phoneNumber, message);
                }
                catch (Exception ex)
                {
                    debugError = ex.Message;
                    System.Diagnostics.Debug.WriteLine("OTP SMS failed: " + ex.Message);

                   
                    using (var cmd = new SqlCommand(
                        "UPDATE LoginOtp SET UsedAt = SYSUTCDATETIME() WHERE OtpID = @OtpId", conn))
                    {
                        cmd.Parameters.AddWithValue("@OtpId", otpId);
                        cmd.ExecuteNonQuery();
                    }
                    return OtpSendResult.SendFailed;
                }
            }

            return OtpSendResult.Sent;
        }

        
        public static OtpVerifyResult Verify(int customerId, string enteredCode)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                int otpId;
                string storedHash;
                int failed;
                using (var cmd = new SqlCommand(
                    @"SELECT TOP 1 OtpID, CodeHash, FailedAttempts
                      FROM LoginOtp
                      WHERE CustomerID = @Id AND UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()
                      ORDER BY OtpID DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return OtpVerifyResult.ExpiredOrMissing;
                        otpId = Convert.ToInt32(r["OtpID"]);
                        storedHash = r["CodeHash"].ToString().Trim();
                        failed = Convert.ToInt32(r["FailedAttempts"]);
                    }
                }

                if (failed >= MaxAttempts) return OtpVerifyResult.Locked;

                if (HashCode(customerId, enteredCode) != storedHash)
                {
                    bool lockNow = failed + 1 >= MaxAttempts;
                    using (var cmd = new SqlCommand(
                        @"UPDATE LoginOtp
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
                    "UPDATE LoginOtp SET UsedAt = SYSUTCDATETIME() WHERE OtpID = @OtpId AND UsedAt IS NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@OtpId", otpId);
                    return cmd.ExecuteNonQuery() == 1 ? OtpVerifyResult.Success : OtpVerifyResult.ExpiredOrMissing;
                }
            }
        }

        public static string GetPhoneNumber(int customerId)
        {
            using (var conn = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand("SELECT PhoneNumber FROM Customer WHERE CustomerID = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", customerId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? null : result.ToString();
            }
        }

        
        public static string LastFourDigits(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return "";
            string digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return digits.Length <= 4 ? digits : digits.Substring(digits.Length - 4);
        }

        private static string RandomCode()
        {
            var buf = new byte[4];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(buf);
            return (BitConverter.ToUInt32(buf, 0) % 1000000).ToString("D6");
        }

        private static string HashCode(int customerId, string code)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(customerId + ":" + code));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}