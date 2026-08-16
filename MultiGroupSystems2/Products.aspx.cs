using MultiGroupSystemsTester.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            const string query = "SELECT ProductID, ProductName, Category, UnitPrice AS Price, Description, QuantityInStock FROM Inventory ORDER BY ProductName";
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
            string productName = gvProducts.DataKeys[rowIndex]["ProductName"].ToString();
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
        
        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            string description = drv["Description"] == DBNull.Value ? "No description available." : drv["Description"].ToString();
            string stock = drv["QuantityInStock"] == DBNull.Value ? "0" : drv["QuantityInStock"].ToString();

            GridViewRow detailRow = new GridViewRow(e.Row.RowIndex, e.Row.RowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
            detailRow.ID = "detailsRow" + e.Row.RowIndex;
            detailRow.Attributes["style"] = "display:none;";

            TableCell cell = new TableCell();
            cell.ColumnSpan = 4;
            cell.Controls.Add(new LiteralControl(
                $"<div class='p-3 bg-light border-start border-3 border-teal'>" +
                $"<p class='mb-1'><strong>Description:</strong> {System.Web.HttpUtility.HtmlEncode(description)}</p>" +
                $"<p class='mb-0'><strong>In stock:</strong> {stock} units</p>" +
                $"</div>"
            ));
            detailRow.Cells.Add(cell);

            int parentIndex = e.Row.Parent.Controls.IndexOf(e.Row);
            e.Row.Parent.Controls.AddAt(parentIndex + 1, detailRow);
        }
    }

}