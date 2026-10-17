using System;
using System.Configuration;
using System.Web.UI;
using MultiGroupSystemsTester.Services;

namespace MultiGroupSystemsTester.Account
{
    public partial class ForgotPassword : Page
    {
        protected void btnSend_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblError.Text = "Please enter your email address.";
                return;
            }

            try
            {
                string token;
                string firstName;
                if (PasswordResetService.CreateRequest(email, out token, out firstName))
                {
                    string link = GetBaseUrl() + ResolveUrl("~/Account/ResetPassword.aspx") + "?token=" + token;
                    EmailService.SendPasswordResetEmail(email, firstName, link);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "Password reset failed: " + ex.ToString()
                );

                // TEMPORARY DEBUG: shows the real error on the page. Remove before submitting.
                lblError.Text = "Password reset failed: " + ex.GetType().Name + " - " + ex.Message
                    + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");

                pnlForm.Visible = true;
                pnlSent.Visible = false;
                return;
            }

            pnlForm.Visible = false;
            pnlSent.Visible = true;
        }

        private string GetBaseUrl()
        {
            string configured = ConfigurationManager.AppSettings["SiteBaseUrl"];
            if (!string.IsNullOrWhiteSpace(configured))
                return configured.Trim().TrimEnd('/');

            return Request.Url.GetLeftPart(UriPartial.Authority);
        }
    }
}