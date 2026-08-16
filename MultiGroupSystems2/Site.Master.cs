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
    }
}