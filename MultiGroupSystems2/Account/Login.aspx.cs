
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using MultiGroupSystemsTester.Services;

namespace MultiGroupSystemsTester.Account
{
    public partial class Login : System.Web.UI.Page
    {
        private const int LegacyPlaintextMaxLength = 50;

        private const string KeyId = "Staff2FA_ID";
        private const string KeyUser = "Staff2FA_User";
        private const string KeyName = "Staff2FA_Name";
        private const string KeyRole = "Staff2FA_Role";
        private const string KeyExpires = "Staff2FA_ExpiresUtc";
        private const int PendingMinutes = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ClearPending();

            ApplyStep();
        }

        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            string connStr = ConfigurationManager
                .ConnectionStrings["GroupWst14ConnectionString"]
                .ConnectionString;

            const string query =
                "SELECT StaffID, FirstName, LastName, Role, Password " +
                "FROM Staff WHERE Username = @Username";

            int staffId;
            string fullName;
            string role;
            string storedPassword;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50)
                        .Value = username;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowError("Incorrect username or password.");
                            return;
                        }

                        staffId = Convert.ToInt32(reader["StaffID"]);

                        fullName =
                            reader["FirstName"].ToString() + " " +
                            reader["LastName"].ToString();

                        role = reader["Role"].ToString();

                        storedPassword =
                            reader["Password"] == DBNull.Value
                                ? ""
                                : reader["Password"].ToString();
                    }
                }

                // IF Staff account has not been activated.
                if (string.IsNullOrEmpty(storedPassword))
                {
                    string activationUrl =
                        ResolveUrl("~/Account/StaffActivate.aspx") +
                        "?username=" + Server.UrlEncode(username);

                    lblError.Text =
                        "Your staff account hasn't been activated yet. " +
                        "You need to create your password first. " +
                        "<a href=\"" +
                        HttpUtility.HtmlAttributeEncode(activationUrl) +
                        "\">Activate your staff account</a>.";

                    lblError.Visible = true;
                    return;
                }

                
                bool passwordOk = VerifyHash(password, storedPassword);
                bool upgradeToHash = false;

             
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
                    UpgradePassword(
                        connStr, staffId, storedPassword, password);
                }

                // Password verified
                
                ClearPending();

                Session[KeyId] = staffId;
                Session[KeyUser] = username;
                Session[KeyName] = fullName;
                Session[KeyRole] = role;
                Session[KeyExpires] =
                    DateTime.UtcNow.AddMinutes(PendingMinutes);

                SendCode(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());

                ClearPending();
                ApplyStep();

                ShowError("Could not log in right now. Please try again.");
            }
        }

     
        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (!HasPending())
            {
                ClearPending();
                ApplyStep();
                ShowError(
                    "Your login session expired. Please sign in again.");
                return;
            }

            int staffId = (int)Session[KeyId];

            switch (StaffOtpService.Verify(
                staffId, txtOtp.Text.Trim()))
            {
                case StaffOtpVerifyResult.Success:
                    CompleteLogin();
                    return;

                case StaffOtpVerifyResult.Wrong:
                    txtOtp.Text = "";
                    ShowError(
                        "That code is incorrect. Please try again.");
                    break;
                case StaffOtpVerifyResult.Locked:
                    ClearPending();
                    ApplyStep();
                    ShowError(
                        "Too many incorrect attempts. " +
                        "Please sign in again to get a new code.");
                    break;

                default:
                    txtOtp.Text = "";
                    ShowError(
                        "That code has expired. " +
                        "Click \"Resend code\" to get a new one.");
                    break;
            }
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            if (!HasPending())
            {
                ClearPending();
                ApplyStep();
                ShowError(
                    "Your login session expired. Please sign in again.");
                return;
            }

            SendCode(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearPending();
            ApplyStep();
        }



        private void SendCode(bool isResend)
        {
            int staffId = (int)Session[KeyId];
            string phone = StaffOtpService.GetPhoneNumber(staffId);

            switch (StaffOtpService.CreateAndSend(staffId, phone, isResend))
            {
                case StaffOtpSendResult.Sent:
                    ApplyStep();
                    lblLastDigits.Text = Server.HtmlEncode(
                        StaffOtpService.LastFourDigits(phone));
                    lblInfo.Text = isResend
                        ? "A new code has been sent."
                        : "We have sent you a verification code.";
                    lblInfo.Visible = true;
                    break;

                case StaffOtpSendResult.TooSoon:
                    ApplyStep();
                    lblLastDigits.Text = Server.HtmlEncode(
                        StaffOtpService.LastFourDigits(phone));
                    ShowError("Please wait a minute before requesting another code.");
                    break;

                case StaffOtpSendResult.NoPhone:
                    ClearPending();
                    ApplyStep();
                    ShowError("No mobile number is saved for this account. Please contact a manager.");
                    break;

                case StaffOtpSendResult.TooManyRequests:
                    ClearPending();
                    ApplyStep();
                    ShowError("Too many codes were requested. Please try again later.");
                    break;

                case StaffOtpSendResult.SmsFailed:
                    ClearPending();
                    ApplyStep();
                    ShowError("We could not send the verification code. Please try again.");
                    break;
            }
        }


        
        private void CompleteLogin()
        {
            string username = (string)Session[KeyUser];
            string fullName = (string)Session[KeyName];
            string role = (string)Session[KeyRole];

            ClearPending();

            Session["AdminFullName"] = fullName;
            Session["AdminRole"] = role;

            FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(
                    1,
                    username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    role,
                    FormsAuthentication.FormsCookiePath
                );

            HttpCookie authCookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket));

            authCookie.HttpOnly = true;
            authCookie.Secure = Request.IsSecureConnection;

            Response.Cookies.Add(authCookie);

            string returnUrl = Request.QueryString["ReturnUrl"];

            Response.Redirect(
                IsLocalUrl(returnUrl)
                    ? returnUrl
                    : "~/Admin/Dashboard.aspx");
        }

        private static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            if (url.StartsWith("~/"))
                return true;

            return url.StartsWith("/") &&
                   !url.StartsWith("//") &&
                   !url.StartsWith("/\\");
        }

        private bool HasPending()
        {
            if (Session[KeyId] == null ||
                Session[KeyExpires] == null)
                return false;

            return DateTime.UtcNow <=
                (DateTime)Session[KeyExpires];
        }

        private void ClearPending()
        {
            Session.Remove(KeyId);
            Session.Remove(KeyUser);
            Session.Remove(KeyName);
            Session.Remove(KeyRole);
            Session.Remove(KeyExpires);
        }

        private void ApplyStep()
        {
            bool otpStep = HasPending();

            pnlCredentials.Visible = !otpStep;
            pnlOtp.Visible = otpStep;

            lblError.Visible = false;
            lblInfo.Visible = false;

            if (otpStep &&
                string.IsNullOrEmpty(lblLastDigits.Text))
            {
                lblLastDigits.Text = Server.HtmlEncode(
                    StaffOtpService.LastFourDigits(
                        StaffOtpService.GetPhoneNumber(
                            (int)Session[KeyId])));
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = Server.HtmlEncode(message);
            lblError.Visible = true;
        }

        
        private static bool VerifyHash(
            string password, string storedHash)
        {
            try
            {
                return PasswordHasher.VerifyPassword(
                    password, storedHash);
            }
            catch
            {
                return false;
            }
        }

        
        private static void UpgradePassword(
            string connStr,
            int staffId,
            string oldValue,
            string plainPassword)
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(
                    "UPDATE Staff SET Password = @New " +
                    "WHERE StaffID = @StaffID AND Password = @Old",
                    conn))
                {
                    cmd.Parameters.Add(
                        "@New", SqlDbType.NVarChar, 200)
                        .Value = PasswordHasher.HashPassword(
                            plainPassword);

                    cmd.Parameters.Add(
                        "@StaffID", SqlDbType.Int)
                        .Value = staffId;

                    cmd.Parameters.Add(
                        "@Old", SqlDbType.NVarChar, 200)
                        .Value = oldValue;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(
                    "Password upgrade failed: " + ex.Message);
            }
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null ||
                a.Length != b.Length)
                return false;

            int diff = 0;

            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}
