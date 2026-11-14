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

            string role = Session["AdminRole"] as string;
            string page = System.IO.Path.GetFileNameWithoutExtension(Request.Path);

            if (!AdminAccess.CanAccess(role, page))
            {
                
                Response.Redirect("~/Admin/AccessDenied.aspx", true);
            }
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