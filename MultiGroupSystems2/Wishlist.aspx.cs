
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using MultiGroupSystemsTester.Models;

namespace MultiGroupSystemsTester
{
    public partial class Wishlist : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager
            .ConnectionStrings["GroupWst14ConnectionString"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Only logged-in customers can access their wishlist
            if (Session["CustomerID"] == null)
            {
                Response.Redirect(
                    "~/Account/CustomerLogin.aspx?ReturnUrl=" +
                    Server.UrlEncode("~/Wishlist.aspx"),
                    false
                );

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                BindWishlist();
            }
        }

        private void BindWishlist()
        {
            int customerId = Convert.ToInt32(Session["CustomerID"]);

            string query = @"
                SELECT
                    i.ProductID,
                    i.ProductName,
                    i.Category,
                    i.UnitPrice,
                    i.QuantityInStock
                FROM Wishlist w
                INNER JOIN Inventory i
                    ON w.ProductID = i.ProductID
                WHERE w.CustomerID = @CustomerID
                ORDER BY w.DateAdded DESC, w.WishlistID DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = customerId;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    gvWishlist.DataSource = dt;
                    gvWishlist.DataBind();
                }
            }
            catch (SqlException)
            {
                ShowMessage("Could not load your wishlist.", true);
            }
        }

        protected void gvWishlist_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("~/Account/CustomerLogin.aspx");
                return;
            }

            if (e.CommandName != "RemoveWishlist" &&
                e.CommandName != "AddToCart")
            {
                return;
            }

            int rowIndex;

            if (!int.TryParse(e.CommandArgument.ToString(),
                out rowIndex))
            {
                return;
            }

            if (rowIndex < 0 || rowIndex >= gvWishlist.Rows.Count)
            {
                return;
            }

            int productId = Convert.ToInt32(
                gvWishlist.DataKeys[rowIndex]["ProductID"]
            );

            if (e.CommandName == "RemoveWishlist")
            {
                RemoveFromWishlist(productId);
            }
            else if (e.CommandName == "AddToCart")
            {
                AddWishlistItemToCart(productId);
            }
        }

        private void RemoveFromWishlist(int productId)
        {
            int customerId = Convert.ToInt32(Session["CustomerID"]);

            string query = @"
                DELETE FROM Wishlist
                WHERE CustomerID = @CustomerID
                  AND ProductID = @ProductID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = customerId;

                    cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                BindWishlist();
                ShowMessage("Product removed from your wishlist.", false);
            }
            catch (SqlException)
            {
                ShowMessage("Could not remove the product.", true);
            }
        }

        private void AddWishlistItemToCart(int productId)
        {
            int customerId = Convert.ToInt32(Session["CustomerID"]);

            // Fetch the latest product details from SQL Server
            string query = @"
                SELECT i.ProductID, i.ProductName,
                       i.UnitPrice, i.QuantityInStock
                FROM Inventory i
                INNER JOIN Wishlist w
                    ON i.ProductID = w.ProductID
                WHERE i.ProductID = @ProductID
                  AND w.CustomerID = @CustomerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = customerId;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            ShowMessage("Product is no longer available.", true);
                            return;
                        }

                        int stock = Convert.ToInt32(
                            reader["QuantityInStock"]
                        );

                        if (stock < 1)
                        {
                            ShowMessage("This product is out of stock.", true);
                            return;
                        }

                        string productName =
                            reader["ProductName"].ToString();

                        decimal price = Convert.ToDecimal(
                            reader["UnitPrice"]
                        );

                        List<CartItem> cart =
                            Session["Cart"] as List<CartItem>
                            ?? new List<CartItem>();

                        CartItem existing = cart.Find(
                            c => c.ProductID == productId
                        );

                        int currentQuantity =
                            existing == null ? 0 : existing.Quantity;

                        if (currentQuantity >= stock)
                        {
                            ShowMessage(
                                "You already have the available stock in your cart.",
                                true
                            );
                            return;
                        }

                        if (existing != null)
                        {
                            existing.Quantity++;
                        }
                        else
                        {
                            cart.Add(new CartItem
                            {
                                ProductID = productId,
                                ProductName = productName,
                                UnitPrice = price,
                                Quantity = 1
                            });
                        }

                        Session["Cart"] = cart;

                        ShowMessage(
                            productName + " added to your cart!",
                            false
                        );
                    }
                }
            }
            catch (SqlException)
            {
                ShowMessage("Could not add the product to your cart.", true);
            }
        }

        private void ShowMessage(string message, bool isError)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = isError
                ? "alert alert-danger d-block mb-3"
                : "alert alert-success d-block mb-3";

            lblMessage.Visible = true;
        }
    }
}
