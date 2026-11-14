using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MultiGroupSystemsTester.Admin
{
    /// <summary>
    /// ONE-TIME use: hashes any plaintext passwords left in the Staff table (for example from
    /// accounts created with the old form that had a password box), using your existing
    /// PasswordHasher so the login keeps working.
    ///
    /// Skipped on purpose:
    ///   - empty/NULL passwords (those accounts are waiting for SMS activation)
    ///   - values that are already in the PasswordHasher format
    ///
    /// Run it once, check the result, then delete the call.
    /// </summary>
    public static class PasswordMigration
    {
        // PasswordHasher format: iterations.base64(16-byte salt).base64(32-byte hash)
        public static bool LooksHashed(string stored)
        {
            if (string.IsNullOrWhiteSpace(stored))
                return false;

            string[] parts = stored.Split('.');
            if (parts.Length != 3)
                return false;

            int iterations;
            if (!int.TryParse(parts[0], out iterations))
                return false;

            try
            {
                return Convert.FromBase64String(parts[1]).Length == 16
                    && Convert.FromBase64String(parts[2]).Length == 32;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static int HashAllPlaintext(string connStr)
        {
            var rows = new List<KeyValuePair<string, string>>();
            int updated = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Username, [Password] FROM Staff", conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        if (rdr["Username"] == DBNull.Value || rdr["Password"] == DBNull.Value)
                            continue;

                        string pw = rdr["Password"].ToString();
                        if (pw.Length == 0 || LooksHashed(pw))
                            continue;

                        rows.Add(new KeyValuePair<string, string>(rdr["Username"].ToString(), pw));
                    }
                }

                foreach (var row in rows)
                {
                    using (SqlCommand upd = new SqlCommand(
                        "UPDATE Staff SET [Password] = @new WHERE Username = @user AND [Password] = @old", conn))
                    {
                        upd.Parameters.AddWithValue("@new", PasswordHasher.HashPassword(row.Value));
                        upd.Parameters.AddWithValue("@user", row.Key);
                        upd.Parameters.AddWithValue("@old", row.Value);
                        updated += upd.ExecuteNonQuery();
                    }
                }
            }

            return updated;
        }
    }
}