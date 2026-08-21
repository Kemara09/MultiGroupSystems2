using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MultiGroupSystemsTester
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateCartCount();
            UpdateCustomerAuthDisplay();
            UpdateStaffAuthDisplay();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/Default.aspx");
        }

        private void UpdateCartCount()
        {
            var cart = Session["Cart"] as System.Collections.Generic.List<MultiGroupSystemsTester.Models.CartItem>;
            int count = cart?.Sum(c => c.Quantity) ?? 0;
            lnkCartCount.InnerText = count > 0 ? $"Cart ({count})" : "Cart";
        }

        private void UpdateCustomerAuthDisplay()
        {
            bool isCustomerLoggedIn = Session["CustomerID"] != null;
            lnkCustomerAuth.Visible = !isCustomerLoggedIn;
            lnkRegister.Visible = !isCustomerLoggedIn;
            btnCustomerLogout.Visible = isCustomerLoggedIn;
            if (isCustomerLoggedIn) btnCustomerLogout.Text = "Log Out (" + Session["CustomerName"] + ")";
        }

        protected void btnCustomerLogout_Click(object sender, EventArgs e)
        {
            Session["CustomerID"] = null;
            Session["CustomerName"] = null;
            Response.Redirect("~/Default.aspx");
        }

        private void UpdateStaffAuthDisplay()
        {
            bool isStaffLoggedIn = false;

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null && !ticket.Expired && ticket.UserData == "Staff")
                    {
                        isStaffLoggedIn = true;
                    }
                }
                catch (Exception)
                {
                    // malformed/tampered cookie — treat as not logged in
                }
            }

            lnkStaffLogin.Visible = !isStaffLoggedIn;
            liStaffMenu.Visible = isStaffLoggedIn;
        }

        protected void btnStaffLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}