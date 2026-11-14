using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

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
                LoadSalesChart(ddlPeriod.SelectedValue);
            }
        }

        protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSalesChart(ddlPeriod.SelectedValue);
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

        private void LoadSalesChart(string period)
        {
            DateTime today = DateTime.Today;
            DateTime start;
            bool monthly;
            string subtitle;

            switch (period)
            {
                case "7d":
                    start = today.AddDays(-6);
                    monthly = false;
                    subtitle = "Daily sales – last 7 days";
                    break;
                case "6m":
                    start = new DateTime(today.Year, today.Month, 1).AddMonths(-5);
                    monthly = true;
                    subtitle = "Monthly sales – last 6 months";
                    break;
                case "ytd":
                    start = new DateTime(today.Year, 1, 1);
                    monthly = true;
                    subtitle = "Monthly sales – this year";
                    break;
                default: // 30d
                    start = today.AddDays(-29);
                    monthly = false;
                    subtitle = "Daily sales – last 30 days";
                    break;
            }
            lblChartSub.Text = subtitle;

            string bucket = monthly
                ? "DATEFROMPARTS(YEAR(OrderDate), MONTH(OrderDate), 1)"
                : "CAST(OrderDate AS date)";

            string query = "SELECT " + bucket + " AS Bucket, SUM(TotalAmount) AS Total " +
                           "FROM [Order] " +
                           "WHERE OrderDate IS NOT NULL AND OrderDate >= @start AND OrderDate < @end " +
                           "GROUP BY " + bucket;

            var found = new Dictionary<DateTime, decimal>();
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", today.AddDays(1));
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        found[Convert.ToDateTime(rdr["Bucket"])] = Convert.ToDecimal(rdr["Total"]);
                }
            }

            // Every day/month in the range gets a bar, with 0 where there were no sales
            var buckets = new List<KeyValuePair<DateTime, decimal>>();
            decimal max = 0;
            for (DateTime d = start; d <= today; d = monthly ? d.AddMonths(1) : d.AddDays(1))
            {
                decimal t;
                found.TryGetValue(d, out t);
                buckets.Add(new KeyValuePair<DateTime, decimal>(d, t));
                if (t > max) max = t;
            }

            decimal axisMax = NiceMax(max);

            DataTable dt = new DataTable();
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("BarHeight", typeof(int));   // int, so SA decimal commas can't break the CSS
            dt.Columns.Add("Tooltip", typeof(string));

            for (int i = 0; i < buckets.Count; i++)
            {
                DateTime d = buckets[i].Key;
                decimal total = buckets[i].Value;

                string label = d.ToString(monthly ? "MMM" : "dd MMM", CultureInfo.InvariantCulture);
                if (period == "30d" && i % 5 != 0) label = "";   // 30 labels would be crowded

                int height = total <= 0 ? 0 : Math.Max(2, (int)Math.Round(total / axisMax * 100));
                string tip = d.ToString(monthly ? "MMMM yyyy" : "dd MMM yyyy", CultureInfo.InvariantCulture)
                             + ": R " + total.ToString("N0");

                dt.Rows.Add(label, height, tip);
            }

            rptSalesChart.DataSource = dt;
            rptSalesChart.DataBind();

            lblY1.Text = FormatRand(axisMax);
            lblY2.Text = FormatRand(axisMax * 0.75m);
            lblY3.Text = FormatRand(axisMax * 0.5m);
            lblY4.Text = FormatRand(axisMax * 0.25m);
            lblY5.Text = "R0";
        }

        // Rounds the top of the axis up to a tidy value (1, 2, 4, 8 or 10 x a power of ten)
        private static decimal NiceMax(decimal value)
        {
            if (value <= 0) return 1000m;
            double mag = Math.Pow(10, Math.Floor(Math.Log10((double)value)));
            double n = (double)value / mag;
            double nice = n <= 1 ? 1 : n <= 2 ? 2 : n <= 4 ? 4 : n <= 8 ? 8 : 10;
            return (decimal)(nice * mag);
        }

        private static string FormatRand(decimal v)
        {
            return v >= 1000
                ? "R" + (v / 1000).ToString("0.#", CultureInfo.InvariantCulture) + "k"
                : "R" + v.ToString("0", CultureInfo.InvariantCulture);
        }
    }
}