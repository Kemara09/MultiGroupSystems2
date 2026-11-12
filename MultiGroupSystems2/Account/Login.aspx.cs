using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace MultiGroupSystemsTester.Account
{
    public partial class Login : System.Web.UI.Page
    {
        // Existing plain-text passwords are short. Hashes are much longer, so anything
        // up to this length is treated as an old plain-text password (see TEMPORARY note).
        private const int LegacyPlaintextMaxLength = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            const string query =
                "SELECT StaffID, FirstName, LastName, Role, Password FROM Staff WHERE Username = @Username";

            try
            {
                int staffId;
                string fullName;
                string role;
                string storedPassword;

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowError("Incorrect username or password.");
                            return;
                        }

                        staffId = Convert.ToInt32(reader["StaffID"]);
                        fullName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                        role = reader["Role"].ToString();
                        storedPassword = reader["Password"] == DBNull.Value ? "" : reader["Password"].ToString();
                    }
                }

                // 1. No password yet: send them to activate
                if (string.IsNullOrEmpty(storedPassword))
                {
                    lblError.Text =
                        "Your staff account hasn't been activated yet. You need to create your password first. " +
                        "<a href=\"" + ResolveUrl("~/Account/StaffActivate.aspx") +
                        "?username=" + Server.UrlEncode(username) + "\">Activate your staff account</a>.";
                    lblError.Visible = true;
                    return;
                }

                // 2. Check the password
                bool passwordOk = VerifyHash(password, storedPassword);
                bool upgradeToHash = false;

                // TEMPORARY: accept an old plain-text password once, then replace it with a hash.
                // Remove this block once every staff password has been converted.
                if (!passwordOk &&
                    storedPassword.Length <= LegacyPlaintextMaxLength &&
                    FixedTimeEquals(storedPassword, password))
                {
                    passwordOk = true;
                    upgradeToHash = true;
                }

                if (!passwordOk)
                {
                    ShowError("Incorrect username or password.");
                    return;
                }

                if (upgradeToHash)
                {
                    UpgradePassword(connStr, staffId, storedPassword, password);
                }

                // 3. Log in
                Session["AdminFullName"] = fullName;
                Session["AdminRole"] = role;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    role,
                    FormsAuthentication.FormsCookiePath
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                {
                    HttpOnly = true,
                    Secure = Request.IsSecureConnection
                };
                Response.Cookies.Add(authCookie);

                // Only follow a ReturnUrl that stays on this site
                string returnUrl = Request.QueryString["ReturnUrl"];
                bool safeReturn =
                    !string.IsNullOrEmpty(returnUrl) &&
                    (returnUrl.StartsWith("~/") ||
                     (returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")));

                Response.Redirect(safeReturn ? returnUrl : "~/Admin/Dashboard.aspx");
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw; // Response.Redirect ends the request this way; let it through
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                ShowError("Could not log in right now. Please try again.");
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
        }

        // ASSUMPTION: your PasswordHasher has a VerifyPassword(password, storedHash) method.
        // Use whatever your CustomerLogin.aspx.cs calls if the name differs.
        private static bool VerifyHash(string password, string storedHash)
        {
            try
            {
                return PasswordHasher.VerifyPassword(password, storedHash);
            }
            catch
            {
                return false; // a plain-text value isn't a valid hash
            }
        }

        private static void UpgradePassword(string connStr, int staffId, string oldValue, string plainPassword)
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(
                    "UPDATE Staff SET Password = @New WHERE StaffID = @StaffID AND Password = @Old", conn))
                {
                    cmd.Parameters.Add("@New", SqlDbType.NVarChar, 200).Value = PasswordHasher.HashPassword(plainPassword);
                    cmd.Parameters.Add("@StaffID", SqlDbType.Int).Value = staffId;
                    cmd.Parameters.Add("@Old", SqlDbType.NVarChar, 200).Value = oldValue;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // The login still succeeds; the password just stays as it was until next time
                System.Diagnostics.Trace.WriteLine("Password upgrade failed: " + ex.Message);
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