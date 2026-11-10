using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MultiGroupSystemsTester.Admin
{
    public partial class Dashboard : AdminPage
    {
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardMetrics();
                LoadRecentOrders();
                LoadSalesChart();
            }
        }

        private void LoadDashboardMetrics()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // 1. Total Products
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Inventory", conn))
                {
                    lblTotalProducts.Text = cmd.ExecuteScalar().ToString();
                }

                // 2. Total Orders
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [Order]", conn))
                {
                    lblTotalOrders.Text = cmd.ExecuteScalar().ToString();
                }

                // 3. Total Staff
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Staff", conn))
                {
                    lblTotalStaff.Text = cmd.ExecuteScalar().ToString();
                }

                // 4. Total Sales
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(TotalAmount), 0) FROM [Order]", conn))
                {
                    object result = cmd.ExecuteScalar();
                    decimal totalSales = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    lblTotalSales.Text = "R " + totalSales.ToString("N0");
                }
            }
        }

        private void LoadRecentOrders()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT TOP 4 
                            o.OrderID, 
                            c.FirstName,
                            c.LastName,
                            o.TotalAmount, 
                            o.OrderStatus,
                            STUFF((SELECT ', ' + i.ProductName 
                                   FROM Order_Line ol 
                                   JOIN Inventory i ON ol.ProductID = i.ProductID 
                                   WHERE ol.OrderID = o.OrderID 
                                   FOR XML PATH('')), 1, 2, '') AS ProductSummary
                         FROM [Order] o
                         JOIN Customer c ON o.CustomerID = c.CustomerID
                         ORDER BY o.OrderID DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        rptRecentOrders.DataSource = dt;
                        rptRecentOrders.DataBind();
                    }
                }
            }
        }

        private void LoadSalesChart()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT 
                                    MONTH(OrderDate) AS MonthNum,
                                    FORMAT(OrderDate, 'MMM') AS MonthName,
                                    SUM(TotalAmount) AS MonthlyTotal
                                 FROM [Order]
                                 WHERE OrderDate IS NOT NULL
                                 GROUP BY MONTH(OrderDate), FORMAT(OrderDate, 'MMM')
                                 ORDER BY MONTH(OrderDate)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        decimal maxTotal = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            decimal total = Convert.ToDecimal(row["MonthlyTotal"]);
                            if (total > maxTotal) maxTotal = total;
                        }

                        dt.Columns.Add("BarHeight", typeof(int));
                        foreach (DataRow row in dt.Rows)
                        {
                            decimal total = Convert.ToDecimal(row["MonthlyTotal"]);
                            int height = (int)((total / maxTotal) * 85) + 10;
                            row["BarHeight"] = height;
                        }

                        rptSalesChart.DataSource = dt;
                        rptSalesChart.DataBind();
                    }
                }
            }
        }
    }
}