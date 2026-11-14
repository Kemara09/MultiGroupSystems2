using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace MultiGroupSystemsTester.Account
{
    public partial class StaffActivate : Page
    {
        private const string GenericNotFound =
            "We couldn't find a staff account waiting to be activated for that username. " +
            "If you think this is a mistake, please contact your manager.";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowStep(1);

                string prefillUsername = Request.QueryString["username"];
                if (!string.IsNullOrEmpty(prefillUsername))
                {
                    txtUsername.Text = prefillUsername;
                }
            }
        }

       
        protected void btnFindAccount_Click(object sender, EventArgs e)
        {
            lblFindError.Text = "";
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                lblFindError.Text = "Please enter your username.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            int staffId;
            string phone;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                const string query =
                    "SELECT StaffID, Password, PhoneNumber FROM Staff WHERE Username = @Username";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblFindError.Text = GenericNotFound;
                            return;
                        }

                        staffId = reader.GetInt32(reader.GetOrdinal("StaffID"));
                        object passwordObj = reader["Password"];
                        bool alreadyActivated = passwordObj != DBNull.Value && !string.IsNullOrEmpty(passwordObj.ToString());
                        phone = reader["PhoneNumber"] == DBNull.Value ? null : reader["PhoneNumber"].ToString();

                       
                        if (reader.Read())
                        {
                            lblFindError.Text = "There is a problem with this account. Please contact your manager.";
                            return;
                        }

                        if (alreadyActivated)
                        {
                           
                            lblFindError.Text = GenericNotFound;
                            return;
                        }
                    }
                }
            }

            switch (StaffOtpService.CreateAndSend(staffId, phone, false))
            {
                case StaffOtpSendResult.Sent:
                    ViewState["ActivateStaffId"] = staffId;
                    lblLastDigits.Text = Server.HtmlEncode(StaffOtpService.LastFourDigits(phone));
                    ShowStep(2);
                    break;

                case StaffOtpSendResult.NoPhone:
                    lblFindError.Text = "There is no mobile number on file for this account, so a verification code can't be sent. Please ask your manager to add one.";
                    break;

                case StaffOtpSendResult.TooManyRequests:
                    lblFindError.Text = "Too many verification codes have been requested for this account. Please try again in an hour.";
                    break;

                default:
                    lblFindError.Text = "We couldn't send the verification code right now. Please check that a valid mobile number is on file, or try again in a moment.";
                    break;
            }
        }

       
        protected void btnVerifyCode_Click(object sender, EventArgs e)
        {
            lblCodeError.Text = "";
            lblCodeInfo.Text = "";

            if (ViewState["ActivateStaffId"] == null)
            {
                lblFindError.Text = "Your session expired. Please start again.";
                ShowStep(1);
                return;
            }

            int staffId = (int)ViewState["ActivateStaffId"];
            string code = txtCode.Text.Trim();

            if (code.Length != 6 || !code.All(char.IsDigit))
            {
                lblCodeError.Text = "Please enter the 6-digit code from your SMS.";
                return;
            }

            switch (StaffOtpService.Verify(staffId, code))
            {
                case StaffOtpVerifyResult.Success:
                    Session["StaffActivationVerifiedID"] = staffId;
                    ShowStep(3);
                    break;

                case StaffOtpVerifyResult.Wrong:
                    txtCode.Text = "";
                    lblCodeError.Text = "That code isn't correct. Please try again.";
                    break;

                case StaffOtpVerifyResult.Locked:
                    ViewState.Remove("ActivateStaffId");
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

            if (ViewState["ActivateStaffId"] == null)
            {
                lblFindError.Text = "Your session expired. Please start again.";
                ShowStep(1);
                return;
            }

            int staffId = (int)ViewState["ActivateStaffId"];
            string phone = StaffOtpService.GetPhoneNumber(staffId);

            switch (StaffOtpService.CreateAndSend(staffId, phone, true))
            {
                case StaffOtpSendResult.Sent:
                    txtCode.Text = "";
                    lblCodeInfo.Text = "A new code has been sent.";
                    break;

                case StaffOtpSendResult.TooSoon:
                    lblCodeError.Text = "Please wait a minute before asking for another code.";
                    break;

                case StaffOtpSendResult.TooManyRequests:
                    lblCodeError.Text = "Too many codes have been requested. Please try again in an hour.";
                    break;

                case StaffOtpSendResult.NoPhone:
                    lblCodeError.Text = "There is no mobile number on file for this account. Please ask your manager to add one.";
                    break;

                default:
                    lblCodeError.Text = "We couldn't send the SMS right now. Please try again in a moment.";
                    break;
            }
        }

        
        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            lblPasswordError.Text = "";

            object verified = Session["StaffActivationVerifiedID"];
            if (verified == null)
            {
                lblFindError.Text = "Your session expired. Please start again.";
                ShowStep(1);
                return;
            }

            int staffId = (int)verified;
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

                
                const string query = @"UPDATE Staff
                                        SET Password = @Password
                                        WHERE StaffID = @StaffID
                                        AND (Password IS NULL OR Password = '')";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@StaffID", staffId);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        lblPasswordError.Text = "This account has already been activated. Please log in instead.";
                        return;
                    }
                }
            }

            Session.Remove("StaffActivationVerifiedID");
            Response.Redirect("~/Account/Login.aspx?activated=1");
        }

        private void ShowStep(int step)
        {
            pnlFindAccount.Visible = step == 1;
            pnlVerifyCode.Visible = step == 2;
            pnlSetPassword.Visible = step == 3;
        }
    }
}