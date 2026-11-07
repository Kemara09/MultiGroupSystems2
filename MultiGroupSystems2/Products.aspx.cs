
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
            string connStr = ConfigurationManager
                .ConnectionStrings["GroupWst14ConnectionString"]
                .ConnectionString;

            const string query =
                "SELECT DISTINCT Category FROM Inventory " +
                "WHERE Category IS NOT NULL ORDER BY Category";

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
                            string category = reader["Category"].ToString();

                            ddlCategory.Items.Add(
                                new ListItem(category, category)
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Categories could not be loaded right now. DEBUG: " +
                    HttpUtility.HtmlEncode(ex.Message);

                lblMessage.Visible = true;
            }
        }

        private void BindProducts(string searchTerm = "", string category = "")
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["GroupWst14ConnectionString"]
                .ConnectionString;

            string query =
                "SELECT ProductID, ProductName, Category, " +
                "UnitPrice AS Price, Description, QuantityInStock, ImagePath " +
                "FROM Inventory WHERE 1=1 ";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += @"
        AND (
            LOWER(REPLACE(REPLACE(ProductName, ' ', ''), '-', ''))
                LIKE @Search
            OR
            LOWER(REPLACE(REPLACE(Category, ' ', ''), '-', ''))
                LIKE @Search
        ) ";
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
                        string normalizedSearch = searchTerm
                            .Trim()
                            .ToLowerInvariant()
                            .Replace(" ", "")
                            .Replace("-", "");

                        cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 255)
                            .Value = "%" + normalizedSearch
                                .Replace("[", "[[]")
                                .Replace("%", "[%]")
                                .Replace("_", "[_]")
                                + "%";
                    }

                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        cmd.Parameters.Add("@Category", SqlDbType.NVarChar, 255)
                            .Value = category;
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    int totalProducts = dt.Rows.Count;

                    int totalPages = Math.Max(
                        1,
                        (int)Math.Ceiling(totalProducts / (double)gvProducts.PageSize)
                    );

                    if (gvProducts.PageIndex >= totalPages)
                    {
                        gvProducts.PageIndex = totalPages - 1;
                    }

                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();

                    int startProduct = totalProducts == 0
                        ? 0
                        : gvProducts.PageIndex * gvProducts.PageSize + 1;

                    int endProduct = Math.Min(
                        totalProducts,
                        (gvProducts.PageIndex + 1) * gvProducts.PageSize
                    );

                    lblProductCount.Text =
                        $"Showing {startProduct}–{endProduct} of {totalProducts} products";

                    lblPageInfo.Text =
                        $"Page {gvProducts.PageIndex + 1} of {totalPages}";

                    btnPrevious.Enabled = gvProducts.PageIndex > 0;
                    btnNext.Enabled = gvProducts.PageIndex < totalPages - 1;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Products could not be loaded right now. DEBUG: " +
                    HttpUtility.HtmlEncode(ex.Message);

                lblMessage.Visible = true;
            }
        }

        protected void gvProducts_PageIndexChanging(
            object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (gvProducts.PageIndex > 0)
            {
                gvProducts.PageIndex--;
                BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (gvProducts.PageIndex < gvProducts.PageCount - 1)
            {
                gvProducts.PageIndex++;
                BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvProducts.PageIndex = 0;
            BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlCategory.SelectedIndex = 0;
            gvProducts.PageIndex = 0;
            BindProducts();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvProducts.PageIndex = 0;
            BindProducts(txtSearch.Text, ddlCategory.SelectedValue);
        }

        protected void gvProducts_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddToWishlist")
            {
                AddProductToWishlist(e);
                return;
            }

            if (e.CommandName != "AddToCart")
                return;

            lblMessage.Visible = false;
            lblCartMsg.Visible = false;

            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            int rowIndex = row.RowIndex;

            int productId = Convert.ToInt32(
                gvProducts.DataKeys[rowIndex]["ProductID"]
            );

            TextBox txtQty = (TextBox)row.FindControl("txtQty");

            if (!int.TryParse(txtQty.Text, out int qty) || qty < 1)
            {
                lblMessage.Text =
                    "Please enter a valid quantity greater than zero.";

                lblMessage.Visible = true;
                return;
            }

            List<CartItem> cart =
                Session["Cart"] as List<CartItem> ??
                new List<CartItem>();

            CartItem existing =
                cart.Find(c => c.ProductID == productId);

            int alreadyInCart =
                existing != null ? existing.Quantity : 0;

            string connStr = ConfigurationManager
                .ConnectionStrings["GroupWst14ConnectionString"]
                .ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT ProductName, UnitPrice, QuantityInStock
                      FROM dbo.Inventory
                      WHERE ProductID = @ProductID", conn))
                {
                    cmd.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblMessage.Text =
                                "This product is no longer available.";

                            lblMessage.Visible = true;
                            return;
                        }

                        string productName =
                            reader["ProductName"].ToString();

                        decimal unitPrice =
                            Convert.ToDecimal(reader["UnitPrice"]);

                        int availableStock =
                            Convert.ToInt32(reader["QuantityInStock"]);

                        if (availableStock <= 0)
                        {
                            lblMessage.Text =
                                "Sorry, this product is out of stock.";

                            lblMessage.Visible = true;
                            return;
                        }

                        if ((long)alreadyInCart + qty > availableStock)
                        {
                            int remaining = Math.Max(
                                0, availableStock - alreadyInCart
                            );

                            lblMessage.Text =
                                $"You cannot add {qty} more of " +
                                $"{HttpUtility.HtmlEncode(productName)}. " +
                                $"You already have {alreadyInCart} in your cart, " +
                                $"and only {remaining} more can be added.";

                            lblMessage.Visible = true;
                            return;
                        }

                        if (existing != null)
                        {
                            existing.Quantity += qty;
                            existing.UnitPrice = unitPrice;
                            existing.ProductName = productName;
                        }
                        else
                        {
                            cart.Add(new CartItem
                            {
                                ProductID = productId,
                                ProductName = productName,
                                UnitPrice = unitPrice,
                                Quantity = qty
                            });
                        }

                        Session["Cart"] = cart;

                        lblCartMsg.Text =
                            $"Added {qty} x " +
                            $"{HttpUtility.HtmlEncode(productName)} to your cart.";

                        lblCartMsg.Visible = true;
                    }
                }
            }
            catch (SqlException)
            {
                lblMessage.Text =
                    "We couldn't check product availability right now. " +
                    "Please try again.";

                lblMessage.Visible = true;
            }
        }

        private void AddProductToWishlist(GridViewCommandEventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect(
                    "~/Account/CustomerLogin.aspx?ReturnUrl=" +
                    Server.UrlEncode("~/Products.aspx"),
                    false
                );

                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            int productId = Convert.ToInt32(
                gvProducts.DataKeys[row.RowIndex]["ProductID"]
            );

            int customerId =
                Convert.ToInt32(Session["CustomerID"]);

            string connStr = ConfigurationManager
                .ConnectionStrings["GroupWst14ConnectionString"]
                .ConnectionString;

            string query = @"
                INSERT INTO Wishlist (CustomerID, ProductID)
                SELECT @CustomerID, @ProductID
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM Wishlist
                    WHERE CustomerID = @CustomerID
                      AND ProductID = @ProductID
                )";

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

                    int rowsAdded = cmd.ExecuteNonQuery();

                    lblCartMsg.Text = rowsAdded > 0
                        ? "Product added to your wishlist!"
                        : "This product is already in your wishlist.";

                    lblCartMsg.Visible = true;
                    lblMessage.Visible = false;
                }
            }
            catch (SqlException)
            {
                lblMessage.Text =
                    "Could not save this product to your wishlist.";

                lblMessage.Visible = true;
                lblCartMsg.Visible = false;
            }
        }

        protected string SafeText(object value)
        {
            if (value == null || value == DBNull.Value)
                return "N/A";

            return HttpUtility.HtmlEncode(value.ToString());
        }
    }
}
