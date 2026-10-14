using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MultiGroupSystemsTester.Account
{
  
    [Serializable]
    public class PendingRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string PasswordHash { get; set; }
    }

    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) ShowStep(1);
        }

        
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            if (!Page.IsValid) return;

            string password = txtPassword.Text;

            string policyError;
            if (!PasswordPolicy.Validate(password, out policyError))
            {
                ShowError(policyError);
                return;
            }

            if (password != txtConfirmPassword.Text)
            {
                ShowError("Passwords do not match.");
                return;
            }

            string email = txtEmail.Text.Trim();

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    if (EmailExists(email, conn))
                    {
                        ShowError("An account with that email address already exists. Try logging in, or use Activate Account if you haven't set a password yet.");
                        return;
                    }
                }
            }
            catch (Exception)
            {
                ShowError("Could not create your account right now.");
                return;
            }

            var pending = new PendingRegistration
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                EmailAddress = email,
                StreetNumber = txtStreetNumber.Text.Trim(),
                StreetName = txtStreetName.Text.Trim(),
                Suburb = txtSuburb.Text.Trim(),
                City = txtCity.Text.Trim(),
                Province = ddlProvince.SelectedValue,
                PostalCode = txtPostalCode.Text.Trim(),
                PasswordHash = PasswordHasher.HashPassword(password)
            };

            string token = RegistrationOtpService.NewToken();
            string debugError;
            OtpSendResult result = RegistrationOtpService.CreateAndSend(token, pending.PhoneNumber, Request.UserHostAddress, false, out debugError);

            switch (result)
            {
                case OtpSendResult.Sent:
                    Session["PendingRegistration"] = pending;
                    Session["PendingRegistrationToken"] = token;
                    lblLastDigits.Text = Server.HtmlEncode(OtpService.LastFourDigits(pending.PhoneNumber));
                    ShowStep(2);
                    break;

                case OtpSendResult.TooManyRequests:
                    ShowError("Too many verification codes have been requested. Please try again in an hour.");
                    break;

                default:
                    
                    ShowError("We couldn't send the verification code right now. Please check your mobile number and try again.");
                    break;
            }
        }

       
        protected void btnVerifyCode_Click(object sender, EventArgs e)
        {
            HideCodeMessages();

            var pending = Session["PendingRegistration"] as PendingRegistration;
            string token = Session["PendingRegistrationToken"] as string;
            if (pending == null || token == null)
            {
                ShowError("Your session expired. Please fill in your details again.");
                ShowStep(1);
                return;
            }

            string code = txtCode.Text.Trim();
            if (code.Length != 6 || !code.All(char.IsDigit))
            {
                ShowCodeError("Please enter the 6-digit code from your SMS.");
                return;
            }

            switch (RegistrationOtpService.Verify(token, code))
            {
                case OtpVerifyResult.Success:
                    CreateAccount(pending);
                    break;

                case OtpVerifyResult.Wrong:
                    txtCode.Text = "";
                    ShowCodeError("That code isn't correct. Please try again.");
                    break;

                case OtpVerifyResult.Locked:
                    ClearPending();
                    ShowError("Too many incorrect codes. Please fill in your details again.");
                    ShowStep(1);
                    break;

                default:
                    ShowCodeError("That code has expired. Click \"Send a new code\" to get another one.");
                    break;
            }
        }

        protected void btnResendCode_Click(object sender, EventArgs e)
        {
            HideCodeMessages();

            var pending = Session["PendingRegistration"] as PendingRegistration;
            string token = Session["PendingRegistrationToken"] as string;
            if (pending == null || token == null)
            {
                ShowError("Your session expired. Please fill in your details again.");
                ShowStep(1);
                return;
            }

            string debugError;
            switch (RegistrationOtpService.CreateAndSend(token, pending.PhoneNumber, Request.UserHostAddress, true, out debugError))
            {
                case OtpSendResult.Sent:
                    txtCode.Text = "";
                    lblCodeInfo.Text = "A new code has been sent.";
                    lblCodeInfo.Visible = true;
                    break;

                case OtpSendResult.TooSoon:
                    ShowCodeError("Please wait a minute before asking for another code.");
                    break;

                case OtpSendResult.TooManyRequests:
                    ShowCodeError("Too many codes have been requested. Please try again in an hour.");
                    break;

                default:
                    
                    ShowCodeError("We couldn't send the SMS right now. Please try again in a moment.");
                    break;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            ShowStep(1);
        }

        
        private void CreateAccount(PendingRegistration pending)
        {
            int newCustomerId;

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    
                    if (EmailExists(pending.EmailAddress, conn))
                    {
                        ClearPending();
                        ShowError("An account with that email address already exists. Try logging in, or use Activate Account if you haven't set a password yet.");
                        ShowStep(1);
                        return;
                    }

                    const string insert = @"
    INSERT INTO [dbo].[Customer]
        (FirstName, LastName, PhoneNumber, EmailAddress, StreetNumber, StreetName, Suburb, City, Province, PostalCode, Password)
    VALUES
        (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @StreetNumber, @StreetName, @Suburb, @City, @Province, @PostalCode, @Password);
    SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", pending.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", pending.LastName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", pending.PhoneNumber);
                        cmd.Parameters.AddWithValue("@EmailAddress", pending.EmailAddress);
                        cmd.Parameters.AddWithValue("@StreetNumber", pending.StreetNumber);
                        cmd.Parameters.AddWithValue("@StreetName", pending.StreetName);
                        cmd.Parameters.AddWithValue("@Suburb", pending.Suburb);
                        cmd.Parameters.AddWithValue("@City", pending.City);
                        cmd.Parameters.AddWithValue("@Province", pending.Province);
                        cmd.Parameters.AddWithValue("@PostalCode", pending.PostalCode);
                        cmd.Parameters.AddWithValue("@Password", pending.PasswordHash);

                        newCustomerId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception)
            {
                ShowCodeError("Could not create your account right now.");
                return;
            }

            ClearPending();
            Session["CustomerID"] = newCustomerId;
            Session["CustomerName"] = pending.FirstName;
            Response.Redirect("~/Default.aspx");
        }

        private static bool EmailExists(string email, SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE EmailAddress = @EmailAddress", conn))
            {
                cmd.Parameters.AddWithValue("@EmailAddress", email);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void ClearPending()
        {
            Session.Remove("PendingRegistration");
            Session.Remove("PendingRegistrationToken");
        }

        private void ShowStep(int step)
        {
            pnlForm.Visible = step == 1;
            pnlVerify.Visible = step == 2;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void ShowCodeError(string message)
        {
            lblCodeError.Text = message;
            lblCodeError.Visible = true;
        }

        private void HideCodeMessages()
        {
            lblCodeError.Visible = false;
            lblCodeInfo.Visible = false;
            lblError.Visible = false;
        }
    }
}