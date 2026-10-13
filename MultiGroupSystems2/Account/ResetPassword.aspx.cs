using System;
using System.Web.UI;

namespace MultiGroupSystemsTester.Account
{
    public partial class ResetPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

           
            if (PasswordResetService.IsTokenValid(Request.QueryString["token"]))
                pnlForm.Visible = true;
            else
                pnlInvalid.Visible = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string password = txtPassword.Text;

            string policyError;
            if (!PasswordPolicy.Validate(password, out policyError))
            {
                lblError.Text = policyError;
                return;
            }

            if (password != txtConfirmPassword.Text)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            
            if (PasswordResetService.CompleteReset(Request.QueryString["token"], password))
            {
                pnlForm.Visible = false;
                pnlDone.Visible = true;
            }
            else
            {
                pnlForm.Visible = false;
                pnlInvalid.Visible = true;
            }
        }
    }
}