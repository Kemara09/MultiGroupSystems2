using MultiGroupSystemsTester.Admin;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MultiGroupSystemsTester
{
    public partial class ViewOrders : AdminPage
    {
        // Use the same connection string name that OrderLogistics.aspx.cs uses
        // Connection string name from Web.config
        private static readonly string ConnString =
            ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatuses();
                BindOrders();
            }
        }

        private void LoadStatuses()
        {
            // Fill the filter dropdown with whatever statuses actually exist in the data
            const string sql = "SELECT DISTINCT OrderStatus FROM dbo.[Order] WHERE OrderStatus IS NOT NULL ORDER BY OrderStatus";

            using (var conn = new SqlConnection(ConnString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string status = reader.GetString(0);
                        ddlStatus.Items.Add(new ListItem(status, status));
                    }
                }
            }
        }

        private void BindOrders()
        {
            lblMessage.Visible = false;

            const string sql = @"
                SELECT o.OrderID, o.CustomerID,
                       ISNULL(c.FirstName + ' ' + c.LastName, 'Unknown customer') AS CustomerName,
                       o.OrderDate, o.TotalAmount, o.PaymentMethod,
                       o.OrderStatus, o.DeliveryStatus, o.DeliveryDate,
                       o.DeliverySuburb, o.DeliveryProvince
                FROM dbo.[Order] o
                LEFT JOIN dbo.Customer c ON c.CustomerID = o.CustomerID
                WHERE (@Status = '' OR o.OrderStatus = @Status)
                  AND (@Search = ''
                       OR CAST(o.OrderID AS varchar(20)) = @Search
                       OR CAST(o.CustomerID AS varchar(20)) = @Search
                       OR c.FirstName LIKE '%' + @Search + '%'
                       OR c.LastName LIKE '%' + @Search + '%'
                       OR (c.FirstName + ' ' + c.LastName) LIKE '%' + @Search + '%')
                ORDER BY o.OrderDate DESC";

            try
            {
                using (var conn = new SqlConnection(ConnString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue ?? "");
                    cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim());

                    var table = new DataTable();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }

                    gvOrders.DataSource = table;
                    gvOrders.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Could not load orders: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvOrders.PageIndex = 0;
            BindOrders();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
            gvOrders.PageIndex = 0;
            BindOrders();
        }

        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            BindOrders();
        }
    }
}