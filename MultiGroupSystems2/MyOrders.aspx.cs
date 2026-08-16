using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MultiGroupSystemsTester
{
    public partial class MyOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("~/Account/CustomerLogin.aspx");
                return;
            }

            if (!IsPostBack) BindOrders();
        }

        private void BindOrders()
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = @"
                SELECT OrderID, OrderDate, TotalAmount, OrderStatus, DeliveryStatus
                FROM [Order]
                WHERE CustomerID = @CustomerID
                ORDER BY OrderDate DESC";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@CustomerID", Session["CustomerID"]);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "You haven't placed any orders yet.";
                    lblMessage.Visible = true;
                    gvOrders.Visible = false;
                }
                else
                {
                    gvOrders.DataSource = dt;
                    gvOrders.DataBind();
                    gvOrders.Visible = true;
                    lblMessage.Visible = false;
                }
            }
        }
    }
}