using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MultiGroupSystemsTester
{
   
    public static class PasswordResetService
    {
        private const int ExpiryMinutes = 30;
        private const int MaxRequestsPerHour = 3;

        private static string ConnStr
        {
            get { return ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString; }
        }

       
        public static bool CreateRequest(string email, out string token, out string firstName)
        {
            token = null;
            firstName = null;

            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();

                int customerId;
                using (var cmd = new SqlCommand(
                    "SELECT CustomerID, FirstName FROM Customer WHERE EmailAddress = @EmailAddress", conn))
                {
                    cmd.Parameters.AddWithValue("@EmailAddress", email);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return false;
                        customerId = Convert.ToInt32(r["CustomerID"]);
                        firstName = r["FirstName"] == DBNull.Value ? "" : r["FirstName"].ToString();
                    }
                }

                using (var cleanup = new SqlCommand(
                    "DELETE FROM PasswordResetRequest WHERE ExpiresAt < DATEADD(DAY, -1, SYSUTCDATETIME())", conn))
                {
                    cleanup.ExecuteNonQuery();
                }

                
                using (var cmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM PasswordResetRequest
                      WHERE CustomerID = @Id AND CreatedAt > DATEADD(HOUR, -1, SYSUTCDATETIME())", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) >= MaxRequestsPerHour) return false;
                }

               
                using (var cmd = new SqlCommand(
                    "UPDATE PasswordResetRequest SET UsedAt = SYSUTCDATETIME() WHERE CustomerID = @Id AND UsedAt IS NULL", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    cmd.ExecuteNonQuery();
                }

                token = RandomHex(32);
                using (var cmd = new SqlCommand(
                    @"INSERT INTO PasswordResetRequest (CustomerID, TokenHash, ExpiresAt)
                      VALUES (@Id, @Hash, DATEADD(MINUTE, @Mins, SYSUTCDATETIME()))", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", customerId);
                    cmd.Parameters.AddWithValue("@Hash", HashToken(token));
                    cmd.Parameters.AddWithValue("@Mins", ExpiryMinutes);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        
        public static bool IsTokenValid(string token)
        {
            if (!IsValidFormat(token)) return false;

            using (var conn = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                @"SELECT COUNT(*) FROM PasswordResetRequest
                  WHERE TokenHash = @Hash AND UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()", conn))
            {
                cmd.Parameters.AddWithValue("@Hash", HashToken(token));
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        
        public static bool CompleteReset(string token, string newPassword)
        {
            if (!IsValidFormat(token)) return false;

            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    object id;
                    using (var cmd = new SqlCommand(
                        @"UPDATE PasswordResetRequest SET UsedAt = SYSUTCDATETIME()
                          OUTPUT inserted.CustomerID
                          WHERE TokenHash = @Hash AND UsedAt IS NULL AND ExpiresAt > SYSUTCDATETIME()", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Hash", HashToken(token));
                        id = cmd.ExecuteScalar();
                    }

                    if (id == null || id == DBNull.Value)
                    {
                        tx.Rollback();
                        return false;
                    }

                    using (var cmd = new SqlCommand(
                        "UPDATE Customer SET Password = @Password WHERE CustomerID = @Id", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Password", PasswordHasher.HashPassword(newPassword));
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(id));
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                    return true;
                }
            }
        }

        private static bool IsValidFormat(string token)
        {
            return !string.IsNullOrEmpty(token) && Regex.IsMatch(token, "^[0-9a-f]{64}$");
        }

        private static string HashToken(string token)
        {
            using (var sha = SHA256.Create())
            {
                return ToHex(sha.ComputeHash(Encoding.UTF8.GetBytes(token)));
            }
        }

        private static string RandomHex(int bytes)
        {
            var buf = new byte[bytes];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(buf);
            return ToHex(buf);
        }

        private static string ToHex(byte[] data)
        {
            var sb = new StringBuilder(data.Length * 2);
            foreach (byte b in data) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}