using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace MultiGroupSystemsTester.Account
{
    public partial class Activate : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowStep(1);

                string prefillEmail = Request.QueryString["email"];
                if (!string.IsNullOrEmpty(prefillEmail))
                {
                    txtEmail.Text = prefillEmail;
                }
            }
        }

        protected void btnFindAccount_Click(object sender, EventArgs e)
        {
            lblFindError.Text = "";
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblFindError.Text = "Please enter your email address.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            int customerId;
            string phone;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                const string query = "SELECT CustomerID, Password, PhoneNumber FROM Customer WHERE EmailAddress = @EmailAddress";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailAddress", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblFindError.Text = "We couldn't find an account with that email address.";
                            return;
                        }

                        customerId = reader.GetInt32(reader.GetOrdinal("CustomerID"));
                        object passwordObj = reader["Password"];
                        bool alreadyActivated = passwordObj != DBNull.Value && !string.IsNullOrEmpty(passwordObj.ToString());

                        if (alreadyActivated)
                        {
                            lblFindError.Text = "This account has already been activated. Please log in instead.";
                            return;
                        }

                        phone = reader["PhoneNumber"] == DBNull.Value ? null : reader["PhoneNumber"].ToString();
                    }
                }
            }

            string debugError;
            OtpSendResult result = OtpService.CreateAndSend(customerId, phone, false, out debugError);

            switch (result)
            {
                case OtpSendResult.Sent:
                    ViewState["ActivateCustomerId"] = customerId;
                    lblLastDigits.Text = Server.HtmlEncode(OtpService.LastFourDigits(phone));
                    ShowStep(2);
                    break;

                case OtpSendResult.NoPhone:
                    lblFindError.Text = "We don't have a mobile number on file for this account, so we can't send a verification code. Please contact us for help.";
                    break;

                case OtpSendResult.TooManyRequests:
                    lblFindError.Text = "Too many verification codes have been requested for this account. Please try again in an hour.";
                    break;

                default:
                   
                    lblFindError.Text = "We couldn't send the verification code right now. Please try again in a moment.";
                    break;
            }
        }

        
        protected void btnVerifyCode_Click(object sender, EventArgs e)
        {
            lblCodeError.Text = "";
            lblCodeInfo.Text = "";

            if (ViewState["ActivateCustomerId"] == null)
            {
                lblFindError.Text = "Your session expired. Please start again.";
                ShowStep(1);
                return;
            }

            int customerId = (int)ViewState["ActivateCustomerId"];
            string code = txtCode.Text.Trim();

            if (code.Length != 6 || !code.All(char.IsDigit))
            {
                lblCodeError.Text = "Please enter the 6-digit code from your SMS.";
                return;
            }

            switch (OtpService.Verify(customerId, code))
            {
                case OtpVerifyResult.Success:
                    
                    Session["ActivationVerifiedCustomerID"] = customerId;
                    ShowStep(3);
                    break;

                case OtpVerifyResult.Wrong:
                    txtCode.Text = "";
                    lblCodeError.Text = "That code isn't correct. Please try again.";
                    break;

                case OtpVerifyResult.Locked:
                    ViewState.Remove("ActivateCustomerId");
                    lblFindError.Text = "Too many incorrect codes. Please start again.";
                    ShowStep(1);
                    break;

                default:
                    lblCodeError.Text = "That code has expired. Click \"Send a new code\" to get another one.";
                    break;
            }
        }

        protected void btnResendCode_Click(object sender, EventArgs e)
        {
            lblCodeError.Text = "";
            lblCodeInfo.Text = "";

            if (ViewState["ActivateCustomerId"] == null)
            {
                lblFindError.Text = "Your session expired. Please start again.";
                ShowStep(1);
                return;
            }

            int customerId = (int)ViewState["ActivateCustomerId"];
            string phone = OtpService.GetPhoneNumber(customerId);

            string debugError;
            switch (OtpService.CreateAndSend(customerId, phone, true, out debugError))
            {
                case OtpSendResult.Sent:
                    txtCode.Text = "";
                    lblCodeInfo.Text = "A new code has been sent.";
                    break;

                case OtpSendResult.TooSoon:
                    lblCodeError.Text = "Please wait a minute before asking for another code.";
                    break;

                case OtpSendResult.TooManyRequests:
                    lblCodeError.Text = "Too many codes have been requested. Please try again in an hour.";
                    break;

                case OtpSendResult.NoPhone:
                    lblCodeError.Text = "We don't have a mobile number on file for this account. Please contact us for help.";
                    break;

                default:
                 
                    lblCodeError.Text = "We couldn't send the SMS right now. Please try again in a moment.";
                    break;
            }
        }

      
        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            lblPasswordError.Text = "";

            object verified = Session["ActivationVerifiedCustomerID"];
            if (verified == null)
            {
                lblFindError.Text = "Your session expired. Please start again.";
                ShowStep(1);
                return;
            }

            int customerId = (int)verified;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            string policyError;
            if (!PasswordPolicy.Validate(password, out policyError))
            {
                lblPasswordError.Text = policyError;
                return;
            }

            if (password != confirmPassword)
            {
                lblPasswordError.Text = "Passwords do not match.";
                return;
            }

            string hashedPassword = PasswordHasher.HashPassword(password);

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
               
                const string query = @"UPDATE Customer
                                        SET Password = @Password
                                        WHERE CustomerID = @CustomerID
                                        AND (Password IS NULL OR Password = '')";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        lblPasswordError.Text = "This account has already been activated. Please log in instead.";
                        return;
                    }
                }
            }

            Session.Remove("ActivationVerifiedCustomerID");
            Response.Redirect("~/Account/CustomerLogin.aspx?activated=1");
        }

        private void ShowStep(int step)
        {
            pnlFindAccount.Visible = step == 1;
            pnlVerifyCode.Visible = step == 2;
            pnlSetPassword.Visible = step == 3;
        }
    }
}