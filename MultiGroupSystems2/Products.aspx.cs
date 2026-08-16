using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MultiGroupSystemsTester.Models;

namespace MultiGroupSystemsTester
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) BindProducts();
        }

        private void BindProducts()
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = "SELECT ProductID, ProductName, Category, UnitPrice AS Price FROM Inventory ORDER BY ProductName";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "Products could not be loaded right now.";
                lblMessage.Visible = true;
            }
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "AddToCart") return;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvProducts.Rows[rowIndex];

            int productId = Convert.ToInt32(gvProducts.DataKeys[rowIndex]["ProductID"]);
            string productName = row.Cells[0].Text;
            decimal unitPrice = Convert.ToDecimal(gvProducts.DataKeys[rowIndex]["Price"]);

            TextBox txtQty = (TextBox)row.FindControl("txtQty");
            if (!int.TryParse(txtQty.Text, out int qty) || qty < 1)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                lblMessage.Visible = true;
                BindProducts();
                return;
            }

            List<CartItem> cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            CartItem existing = cart.Find(c => c.ProductID == productId);
            if (existing != null)
                existing.Quantity += qty;
            else
                cart.Add(new CartItem { ProductID = productId, ProductName = productName, UnitPrice = unitPrice, Quantity = qty });

            Session["Cart"] = cart;

            lblCartMsg.Text = $"Added {qty} x {productName} to your cart.";
            lblCartMsg.Visible = true;
            BindProducts();
        }
    }
}