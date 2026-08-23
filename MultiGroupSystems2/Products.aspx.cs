using MultiGroupSystemsTester.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MultiGroupSystemsTester
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategories();
                BindProducts();
            }
        }

        private void BindCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = "SELECT DISTINCT Category FROM Inventory WHERE Category IS NOT NULL ORDER BY Category";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ddlCategory.Items.Add(new ListItem(reader["Category"].ToString(), reader["Category"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Categories could not be loaded right now. DEBUG: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        private void BindProducts(string searchTerm = "", string category = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            string query = "SELECT ProductID, ProductName, Category, UnitPrice AS Price, Description, QuantityInStock " +
                            "FROM Inventory WHERE 1=1 ";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += "AND (ProductName LIKE @Search OR Category LIKE @Search) ";
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query += "AND Category = @Category ";
            }

            query += "ORDER BY ProductName";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchTerm.Trim() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        cmd.Parameters.AddWithValue("@Category", category);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Products could not be loaded right now. DEBUG: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlCategory.SelectedIndex = 0;
            BindProducts();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "AddToCart") return;

            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int rowIndex = row.RowIndex;

            int productId = Convert.ToInt32(gvProducts.DataKeys[rowIndex]["ProductID"]);
            string productName = gvProducts.DataKeys[rowIndex]["ProductName"].ToString();
            decimal unitPrice = Convert.ToDecimal(gvProducts.DataKeys[rowIndex]["Price"]);

            TextBox txtQty = (TextBox)row.FindControl("txtQty");
            if (!int.TryParse(txtQty.Text, out int qty) || qty < 1)
            {
                lblMessage.Text = "Please enter a valid quantity.";
                lblMessage.Visible = true;
                BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
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
            BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
        }

        protected string SafeText(object value)
        {
            if (value == null || value == DBNull.Value) return "N/A";
            return HttpUtility.HtmlEncode(value.ToString());
        }
    }
}