using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace MultiGroupSystemsTester.Admin
{
    public class AdminPage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureStaffLoggedIn();
        }

        private void EnsureStaffLoggedIn()
        {
            bool isStaffLoggedIn = false;

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired && !string.IsNullOrEmpty(ticket.UserData))
                    {
                        isStaffLoggedIn = true;
                    }
                }
                catch (Exception)
                {
                    // malformed/tampered cookie — treat as not logged in
                }
            }

            if (!isStaffLoggedIn)
            {
                string returnUrl = HttpUtility.UrlEncode(Request.Url.PathAndQuery);
                Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + returnUrl, endResponse: true);
            }
        }
    }
}