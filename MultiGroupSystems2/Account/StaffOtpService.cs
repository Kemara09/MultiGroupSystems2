using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MultiGroupSystemsTester.Account
{
    public enum StaffOtpSendResult { Sent, NoPhone, TooSoon, TooManyRequests, SmsFailed }

    public enum StaffOtpVerifyResult { Success, Wrong, Locked, Expired }

    public static class StaffOtpService
    {
        private const int CodeLifetimeMinutes = 5;
        private const int MaxCodesPerHour = 5;
        private const int ResendCooldownSeconds = 60;
        private const int MaxWrongAttempts = 5;

        private static string ConnStr =>
            ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

      
        public static StaffOtpSendResult CreateAndSend(int staffId, string phone, bool isResend)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return StaffOtpSendResult.NoPhone;

            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                // Limit on codes
                using (var cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM StaffOtp WHERE StaffID = @StaffID AND CreatedAt > DATEADD(HOUR, -1, GETDATE())", conn))
                {
                    cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;
                    if ((int)cmd.ExecuteScalar() >= MaxCodesPerHour)
                        return StaffOtpSendResult.TooManyRequests;
                }

                // Cooldown between resends
                if (isResend)
                {
                    using (var cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM StaffOtp WHERE StaffID = @StaffID AND CreatedAt > DATEADD(SECOND, -@Secs, GETDATE())", conn))
                    {
                        cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;
                        cmd.Parameters.Add("@Secs", SqlDbType.Int).Value = ResendCooldownSeconds;
                        if ((int)cmd.ExecuteScalar() > 0)
                            return StaffOtpSendResult.TooSoon;
                    }
                }

               
                using (var cmd = new SqlCommand(
                    "UPDATE StaffOtp SET Used = 1 WHERE StaffID = @StaffID AND Used = 0", conn))
                {
                    cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;
                    cmd.ExecuteNonQuery();
                }

                string code = GenerateCode();
                int otpId;

                using (var cmd = new SqlCommand(
                    @"INSERT INTO StaffOtp (StaffID, CodeHash, ExpiresAt)
                      VALUES (@StaffID, @CodeHash, DATEADD(MINUTE, @Mins, GETDATE()));
                      SELECT SCOPE_IDENTITY();", conn))
                {
                    cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;
                    cmd.Parameters.Add("@CodeHash", SqlDbType.NVarChar, 64).Value = Hash(code, staffId);
                    cmd.Parameters.Add("@Mins", SqlDbType.Int).Value = CodeLifetimeMinutes;
                    otpId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                bool sent = false;
                try
                {
                    string message =
                        "Multigroup Inc.\nYour staff activation code is " + code +
                        ". It expires in " + CodeLifetimeMinutes + " minutes. Do not share it.";

                    sent = MultiGroupSystemsTester.Services.SmsService.SendSms(ToInternational(phone), message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("STAFF OTP SMS ERROR: " + ex.Message);
                }

                if (!sent)
                {
                    // Prevents unused verfication codes from working
                    using (var cmd = new SqlCommand("UPDATE StaffOtp SET Used = 1 WHERE StaffOtpID = @Id", conn))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = otpId;
                        cmd.ExecuteNonQuery();
                    }
                    return StaffOtpSendResult.SmsFailed;
                }

                return StaffOtpSendResult.Sent;
            }
        }

       
        public static StaffOtpVerifyResult Verify(int staffId, string code)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                int otpId;
                string storedHash;
                int attempts;
                bool expired;

                using (var cmd = new SqlCommand(
                    @"SELECT TOP 1 StaffOtpID, CodeHash, Attempts,
                             CASE WHEN ExpiresAt < GETDATE() THEN 1 ELSE 0 END AS IsExpired
                      FROM StaffOtp
                      WHERE StaffID = @StaffID AND Used = 0
                      ORDER BY CreatedAt DESC", conn))
                {
                    cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;

                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                            return StaffOtpVerifyResult.Expired;

                        otpId = r.GetInt32(0);
                        storedHash = r.GetString(1);
                        attempts = r.GetInt32(2);
                        expired = r.GetInt32(3) == 1;
                    }
                }

                if (expired)
                    return StaffOtpVerifyResult.Expired;

                if (attempts >= MaxWrongAttempts)
                    return StaffOtpVerifyResult.Locked;

                if (FixedTimeEquals(storedHash, Hash(code, staffId)))
                {
                    using (var cmd = new SqlCommand("UPDATE StaffOtp SET Used = 1 WHERE StaffOtpID = @Id", conn))
                    {
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = otpId;
                        cmd.ExecuteNonQuery();
                    }
                    return StaffOtpVerifyResult.Success;
                }

                // Wrong code
                using (var cmd = new SqlCommand(
                    @"UPDATE StaffOtp
                      SET Attempts = Attempts + 1,
                          Used = CASE WHEN Attempts + 1 >= @Max THEN 1 ELSE Used END
                      WHERE StaffOtpID = @Id", conn))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = otpId;
                    cmd.Parameters.Add("@Max", SqlDbType.Int).Value = MaxWrongAttempts;
                    cmd.ExecuteNonQuery();
                }

                return attempts + 1 >= MaxWrongAttempts
                    ? StaffOtpVerifyResult.Locked
                    : StaffOtpVerifyResult.Wrong;
            }
        }

       
        public static string GetPhoneNumber(int staffId)
        {
            using (var conn = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand("SELECT PhoneNumber FROM Staff WHERE StaffID = @StaffID", conn))
            {
                cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;
                conn.Open();
                object o = cmd.ExecuteScalar();
                return o == null || o == DBNull.Value ? null : o.ToString();
            }
        }

        public static string LastFourDigits(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "";
            string digits = new string(phone.Where(char.IsDigit).ToArray());
            return digits.Length <= 4 ? digits : digits.Substring(digits.Length - 4);
        }

        private static string ToInternational(string phone)
        {
            string trimmed = phone.Trim();
            bool hasPlus = trimmed.StartsWith("+");

            
            string digits = new string(trimmed.Where(char.IsDigit).ToArray());

            if (hasPlus) return "+" + digits;
            if (digits.StartsWith("27") && digits.Length >= 11) return "+" + digits;

            return "+27" + digits.TrimStart('0');
        }

        private static string GenerateCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);
                uint value = BitConverter.ToUInt32(bytes, 0) % 1000000;
                return value.ToString("D6");
            }
        }

        private static string Hash(string code, int staffId)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(code + ":" + staffId));
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}