using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MultiGroupSystemsTester.Models;

namespace MultiGroupSystemsTester
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindCart();
        }

        private void BindCart()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                cartRow.Visible = false;
                lblEmpty.Visible = true;
                rptCart.DataSource = null;
                rptCart.DataBind();
                lblTotal.Text = (0m).ToString("C");
                return;
            }

            lblEmpty.Visible = false;
            cartRow.Visible = true;
            rptCart.DataSource = cart;
            rptCart.DataBind();
            lblTotal.Text = "R " + cart.Sum(c => c.Subtotal).ToString("N2");

            // Route checkout link based on login state
            if (Session["CustomerID"] == null)
            {
                lnkCheckout.NavigateUrl = "~/Account/CustomerLogin.aspx?ReturnUrl=" + Server.UrlEncode("~/Checkout.aspx");
            }
            else
            {
                lnkCheckout.NavigateUrl = "~/Checkout.aspx";
            }
        }

        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            if (index < 0 || index >= cart.Count)
            {
                BindCart();
                return;
            }

            switch (e.CommandName)
            {
                case "Increase":
                    cart[index].Quantity += 1;
                    break;

                case "Decrease":
                    if (cart[index].Quantity > 1)
                        cart[index].Quantity -= 1;
                    break;

                case "Remove":
                    cart.RemoveAt(index);
                    break;
            }

            Session["Cart"] = cart;
            BindCart();
        }
    }
}