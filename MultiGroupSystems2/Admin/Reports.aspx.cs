using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using MultiGroupSystemsTester.Admin;
using Newtonsoft.Json;

namespace MultiGroupSystemsTester
{
    public partial class Reports : AdminPage
    {
        private const string ConnectionName = "GroupWst14ConnectionString";
        private const int PageSize = 50;

        protected readonly Dictionary<string, string> ReportLinks =
            new Dictionary<string, string>
            {
                { "profit", "Profit & Loss" },
                { "vat", "VAT" },
                { "cash", "Cash Flow" },
                { "orders", "All Orders" },
                { "delays", "Pending & Delayed Orders" },
                { "receivables", "Accounts Receivable" },
                { "customers", "Top Customers" },
                { "queries", "Customer Queries" }
            };

        protected readonly Dictionary<string, string> CustomMetrics =
            new Dictionary<string, string>
            {
                { "orders-month", "Order count by month" },
                { "value-month", "Recorded order value by month" },
                { "returns-month", "Returned orders by month" },
                { "orders-status", "Order count by status" },
                { "queries-month", "Customer queries by month" }
            };

        protected readonly Dictionary<string, string> ChartTypes =
            new Dictionary<string, string>
            {
                { "bar", "Column chart" },
                { "line", "Line chart" },
                { "pie", "Pie chart" },
                { "doughnut", "Doughnut chart" }
            };

        protected sealed class ReportChart
        {
            public string Title { get; set; }
            public DataTable Values { get; set; }
            public string Type { get; set; }
            public bool Currency { get; set; }
            public bool Horizontal { get; set; }
        }

        protected readonly List<ReportChart> Charts = new List<ReportChart>();
        protected DataTable Details { get; private set; }
        protected DataTable CustomReportLinks { get; private set; }
        protected string CustomLinksError { get; private set; }
        protected string ActiveReport { get; private set; }
        protected string ReportTitle { get; private set; }
        protected string ReportNote { get; private set; }
        protected string LoadError { get; private set; }
        protected string SaveError { get; private set; }
        protected string EnteredTitle { get; private set; }
        protected string EnteredMetric { get; private set; }
        protected string EnteredChartType { get; private set; }
        protected int OpenReportId { get; private set; }
        protected int PageNumber { get; private set; }
        protected bool HasNextPage { get; private set; }

        protected string PageLink
        {
            get
            {
                return OpenReportId > 0
                    ? "?saved=" + OpenReportId.ToString(CultureInfo.InvariantCulture)
                    : "?report=" + ActiveReport;
            }
        }

        protected string SaveToken
        {
            get
            {
                const string key = "BusinessReportSaveToken";
                if (Session[key] == null)
                    Session[key] = Guid.NewGuid().ToString("N");
                return (string)Session[key];
            }
        }

        private const string StatusSql = @"
            SELECT COALESCE(NULLIF(LTRIM(RTRIM(OrderStatus)), ''), 'Unspecified') AS [Order status],
                   COUNT_BIG(*) AS [Orders]
            FROM dbo.[Order]
            GROUP BY COALESCE(NULLIF(LTRIM(RTRIM(OrderStatus)), ''), 'Unspecified')
            ORDER BY [Orders] DESC, [Order status]";

        protected void Page_Load(object sender, EventArgs e)
        {
            int page;
            PageNumber = int.TryParse(Request.QueryString["p"], out page) && page > 0
                ? page : 1;

            string route = Request.QueryString["report"];
            int savedId;
            OpenReportId = int.TryParse(Request.QueryString["saved"], out savedId) && savedId > 0
                ? savedId : 0;

            DateTime today = DateTime.UtcNow.AddHours(2).Date;
            DateTime month = new DateTime(today.Year, today.Month, 1);
            DateTime start = month.AddMonths(-5);
            DateTime end = month.AddMonths(1);

            try
            {
                using (SqlConnection connection = OpenConnection())
                {
                    if (connection == null)
                    {
                        LoadError = "The reports database connection has not been configured.";
                        return;
                    }

                    LoadCustomReportLinks(connection);

                    if (OpenReportId > 0)
                    {
                        DataTable saved = Query(connection, @"
                            SELECT ReportTitle, ReportCategory
                            FROM dbo.BusinessReports
                            WHERE ReportID = @id AND ReportStatus = N'Saved'",
                            new SqlParameter("@id", SqlDbType.Int) { Value = OpenReportId });

                        if (saved.Rows.Count == 0)
                        {
                            LoadError = "The saved report could not be found.";
                            return;
                        }

                        ReportTitle = (string)saved.Rows[0]["ReportTitle"];
                        route = (string)saved.Rows[0]["ReportCategory"];

                        if (route.StartsWith("custom:", StringComparison.Ordinal))
                        {
                            ActiveReport = "custom";
                            LoadCustomReport(connection, route, start, end);
                            return;
                        }

                        if (!ReportLinks.ContainsKey(route))
                        {
                            LoadError = "This saved report uses an unsupported report type.";
                            return;
                        }
                    }

                    if (route == "create")
                    {
                        ActiveReport = "create";
                        ReportTitle = "Create Report";
                        ReportNote = "Choose a calculation and chart to add a new report to Browse reports.";
                        return;
                    }

                    if (route == "saved")
                    {
                        ActiveReport = "saved";
                        ReportTitle = "Saved Reports";
                        ReportNote = "Open a report to run it with current data. Created times are UTC.";
                        LoadDetails(connection, @"
                            SELECT ReportID AS [Report ID], ReportTitle AS [Report name],
                                   CASE WHEN ReportCategory LIKE N'custom:%' THEN N'Custom'
                                        ELSE ReportCategory END AS [Report type],
                                   CreatedDate AS [Created UTC]
                            FROM dbo.BusinessReports
                            WHERE ReportStatus = N'Saved'
                              AND (ReportCategory LIKE N'custom:%' OR ReportCategory IN
                                   (N'profit', N'vat', N'cash', N'orders', N'delays',
                                    N'receivables', N'customers', N'queries'))
                            ORDER BY CreatedDate DESC, ReportID DESC");
                        return;
                    }

                    string title;
                    if (route != null && ReportLinks.TryGetValue(route, out title))
                    {
                        ActiveReport = route;
                        ReportTitle = ReportTitle ?? title;
                    }

                    LoadReport(connection, start, end, today);
                }
            }
            catch (SqlException exception)
            {
                LogSqlError(exception);
                LoadError = "The report data could not be loaded. Check the database connection and table permissions.";
            }
        }

        private void LoadCustomReportLinks(SqlConnection connection)
        {
            try
            {
                CustomReportLinks = Query(connection, @"
                    SELECT ReportID, ReportTitle
                    FROM dbo.BusinessReports
                    WHERE ReportStatus = N'Saved'
                      AND ReportCategory LIKE N'custom:%'
                    ORDER BY ReportTitle, ReportID");
            }
            catch (SqlException exception)
            {
                LogSqlError(exception);
                CustomLinksError = "Custom report links could not be loaded.";
            }
        }

        protected void SaveReport_Click(object sender, EventArgs e)
        {
            if (ActiveReport != "create" || LoadError != null)
                return;

            if (!string.Equals(Request.Form["reportToken"], SaveToken, StringComparison.Ordinal))
            {
                SaveError = "This form has expired. Reload the page and try again.";
                return;
            }

            EnteredTitle = (Request.Form["reportTitle"] ?? "").Trim();
            EnteredMetric = Request.Form["reportMetric"];
            EnteredChartType = Request.Form["reportChart"];

            if (EnteredTitle.Length == 0 || EnteredTitle.Length > 200 ||
                !IsCustomReportDefinition(EnteredMetric, EnteredChartType))
            {
                SaveError = "Enter a name up to 200 characters and choose a valid calculation and chart.";
                return;
            }

            string definition = "custom:" + EnteredMetric + ":" + EnteredChartType;
            int reportId;

            try
            {
                using (SqlConnection connection = OpenConnection())
                {
                    if (connection == null)
                    {
                        SaveError = "The reports database connection has not been configured.";
                        return;
                    }

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    using (var command = new SqlCommand(@"
                        DECLARE @created TABLE (ReportID int);

                        IF EXISTS (
                            SELECT 1 FROM sys.columns
                            WHERE object_id = OBJECT_ID(N'dbo.BusinessReports')
                              AND name = N'ReportID'
                              AND (is_identity = 1 OR default_object_id <> 0)
                        )
                        BEGIN
                            INSERT INTO dbo.BusinessReports
                                (ReportTitle, ReportCategory, ReportStatus, CreatedDate)
                            OUTPUT inserted.ReportID INTO @created
                            VALUES (@title, @category, N'Saved', SYSUTCDATETIME());
                        END
                        ELSE
                        BEGIN
                            DECLARE @id int;
                            SELECT @id = ISNULL(MAX(ReportID), 0) + 1
                            FROM dbo.BusinessReports WITH (TABLOCKX, HOLDLOCK);

                            INSERT INTO dbo.BusinessReports
                                (ReportID, ReportTitle, ReportCategory, ReportStatus, CreatedDate)
                            OUTPUT inserted.ReportID INTO @created
                            VALUES (@id, @title, @category, N'Saved', SYSUTCDATETIME());
                        END

                        SELECT ReportID FROM @created;", connection, transaction))
                    {
                        command.Parameters.Add("@title", SqlDbType.NVarChar, 200).Value = EnteredTitle;
                        command.Parameters.Add("@category", SqlDbType.NVarChar, 100).Value = definition;
                        reportId = (int)command.ExecuteScalar();
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException exception)
            {
                LogSqlError(exception);
                SaveError = "The report could not be saved. Check the BusinessReports table permissions and constraints.";
                return;
            }

            Response.Redirect("Reports.aspx?saved=" +
                reportId.ToString(CultureInfo.InvariantCulture), false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private bool IsCustomReportDefinition(string metric, string chartType)
        {
            return metric != null && chartType != null &&
                   CustomMetrics.ContainsKey(metric) && ChartTypes.ContainsKey(chartType) &&
                   (metric != "value-month" || chartType == "bar" || chartType == "line");
        }

        private void LoadCustomReport(SqlConnection connection, string definition,
            DateTime start, DateTime end)
        {
            string[] parts = definition.Split(':');
            if (parts.Length != 3 || !IsCustomReportDefinition(parts[1], parts[2]))
            {
                LoadError = "This custom report has an invalid definition.";
                return;
            }

            string metric = parts[1];
            ReportNote = "Shows the current month and the previous five calendar months.";

            switch (metric)
            {
                case "orders-status":
                    Details = Query(connection, StatusSql);
                    ReportNote = "Counts all recorded orders by their current status.";
                    break;
                case "queries-month":
                    Details = Monthly(connection, start, end, "queries");
                    break;
                case "value-month":
                    Details = Monthly(connection, start, end, "value");
                    break;
                case "returns-month":
                    Details = Monthly(connection, start, end, "returns");
                    ReportNote = "Counts orders currently marked Returned in the last six calendar months, grouped by their original order month.";
                    break;
                default:
                    Details = Monthly(connection, start, end, "count");
                    break;
            }

            AddChart(CustomMetrics[metric], Details, parts[2], metric == "value-month");
        }

        private void LoadReport(SqlConnection connection, DateTime start, DateTime end,
            DateTime today)
        {
            switch (ActiveReport)
            {
                case "profit":
                case "vat":
                    LoadSalesTaxSummary(connection, start, end);
                    break;

                case "cash":
                    LoadCashEstimate(connection, start, end);
                    break;

                case "receivables":
                    LoadReceivables(connection, today);
                    break;

                case "delays":
                    LoadDeliveryExceptions(connection, today);
                    break;

                case "orders":
                    AddChart("Order status breakdown", Query(connection, StatusSql), "doughnut");
                    LoadDetails(connection, @"
                        SELECT OrderID AS [Order ID], CustomerID AS [Customer ID],
                               OrderDate AS [Order date], TotalAmount AS [Order total],
                               OrderStatus AS [Order status], DeliveryStatus AS [Delivery status],
                               DeliveryDate AS [Expected delivery], PaymentMethod AS [Payment method]
                        FROM dbo.[Order]
                        ORDER BY OrderDate DESC, OrderID DESC");
                    break;

                case "customers":
                    ReportNote = "Ranks customers by recorded order value across all statuses. Customers are shown by ID.";
                    Details = Query(connection, @"
                        SELECT TOP (10) CustomerID AS [Customer ID],
                               SUM(TotalAmount) AS [Order value], COUNT_BIG(*) AS [Orders]
                        FROM dbo.[Order]
                        WHERE CustomerID IS NOT NULL AND TotalAmount IS NOT NULL
                        GROUP BY CustomerID
                        ORDER BY [Order value] DESC, CustomerID");
                    AddChart("Top 10 customers by order value", Details, "bar", true, true);
                    break;

                case "queries":
                    AddChart("Customer queries by month", Monthly(connection, start, end, "queries"), "line");
                    LoadDetails(connection, @"
                        SELECT ID AS [Query ID], FirstName AS [First name], Surname,
                               Email, Message, StaffID AS [Staff ID], SubmissionDate AS [Received]
                        FROM dbo.ContactSubmissions
                        ORDER BY SubmissionDate DESC, ID DESC");
                    break;

                default:
                    AddChart("Order value by month", Monthly(connection, start, end, "value"), "bar", true);
                    AddChart("Order status breakdown", Query(connection, StatusSql), "doughnut");
                    AddChart("Customer queries by month", Monthly(connection, start, end, "queries"), "line");
                    break;
            }
        }

        private static DataTable Monthly(SqlConnection connection, DateTime start,
            DateTime end, string metric)
        {
            string date = "OrderDate";
            string table = "dbo.[Order]";
            string value = "COUNT_BIG(*) AS [Orders]";
            string filter = "";

            switch (metric)
            {
                case "queries":
                    date = "SubmissionDate";
                    table = "dbo.ContactSubmissions";
                    value = "COUNT_BIG(*) AS [Customer queries]";
                    break;
                case "value":
                    value = "SUM(TotalAmount) AS [Recorded order value]";
                    filter = " AND TotalAmount IS NOT NULL";
                    break;
                case "returns":
                    value = "COUNT_BIG(*) AS [Returned orders]";
                    filter = " AND LOWER(LTRIM(RTRIM(OrderStatus))) = 'returned'";
                    break;
            }

            // SQL fragments above are fixed by the application.
            string sql = "SELECT CONVERT(char(7), " + date + ", 120) AS [Month], " + value +
                         " FROM " + table + " WHERE " + date + " >= @start AND " + date + " < @end" +
                         filter + " GROUP BY CONVERT(char(7), " + date + ", 120) ORDER BY [Month]";

            return Query(connection, sql, DateParameter("@start", start), DateParameter("@end", end));
        }

        private void LoadSalesTaxSummary(SqlConnection connection, DateTime start, DateTime end)
        {
            bool vat = ActiveReport == "vat";
            ReportNote = vat
                ? "Estimates output VAT over six calendar months. Assumes all included line subtotals include VAT at 15%. Purchase VAT is unavailable."
                : "Shows revenue over six calendar months under the VAT-inclusive subtotal assumption. Profit is unavailable without costs and expenses.";

            Details = Query(connection, @"
                WITH MonthlySales AS (
                    SELECT CONVERT(char(7), o.OrderDate, 120) AS [Month],
                           COUNT(DISTINCT o.OrderID) AS [Orders],
                           SUM(CAST(ol.Subtotal AS decimal(19,4))) AS Gross
                    FROM dbo.[Order] AS o
                    JOIN dbo.Order_Line AS ol ON ol.OrderID = o.OrderID
                    WHERE o.OrderDate >= @start AND o.OrderDate < @end
                      AND LOWER(LTRIM(RTRIM(COALESCE(o.OrderStatus, ''))))
                          NOT IN ('cancelled', 'canceled', 'refunded', 'returned')
                      AND ol.Subtotal IS NOT NULL
                    GROUP BY CONVERT(char(7), o.OrderDate, 120)
                )
                SELECT [Month], [Orders], CAST(Gross AS decimal(19,2)) AS [Sales incl. VAT],
                       CAST(Gross / 1.15 AS decimal(19,2)) AS [Sales excl. VAT],
                       CAST(Gross - ROUND(Gross / 1.15, 2) AS decimal(19,2)) AS [Estimated output VAT]
                FROM MonthlySales
                ORDER BY [Month]",
                DateParameter("@start", start), DateParameter("@end", end));

            AddMonthlyAmountChart(vat ? "Estimated output VAT by month" : "Revenue excluding VAT by month",
                vat ? "Estimated output VAT" : "Sales excl. VAT", "bar");
        }

        private void LoadCashEstimate(SqlConnection connection, DateTime start, DateTime end)
        {
            ReportNote = "Estimates receipts from Paid and Completed orders, grouped by order date. Payment dates, partial receipts and cash outflows are unavailable.";

            Details = Query(connection, @"
                SELECT CONVERT(char(7), OrderDate, 120) AS [Month],
                       SUM(CASE WHEN StatusValues.OrderState IN ('paid', 'completed')
                                THEN TotalAmount ELSE 0 END) AS [Estimated receipts],
                       SUM(CASE WHEN StatusValues.OrderState IN ('pending', 'unpaid', 'partial')
                                THEN TotalAmount ELSE 0 END) AS [Order value flagged for review],
                       SUM(TotalAmount) AS [Recorded order value]
                FROM dbo.[Order]
                CROSS APPLY (VALUES (LOWER(LTRIM(RTRIM(COALESCE(OrderStatus, ''))))))
                    AS StatusValues(OrderState)
                WHERE OrderDate >= @start AND OrderDate < @end
                  AND TotalAmount IS NOT NULL
                  AND StatusValues.OrderState NOT IN ('cancelled', 'canceled', 'refunded', 'returned')
                GROUP BY CONVERT(char(7), OrderDate, 120)
                ORDER BY [Month]",
                DateParameter("@start", start), DateParameter("@end", end));

            AddMonthlyAmountChart("Estimated receipts by order month", "Estimated receipts", "line");
        }

        private void LoadReceivables(SqlConnection connection, DateTime today)
        {
            ReportNote = "Flags Pending, Unpaid, Partial and Processing orders for review. Amounts are full order values. Remaining balances are unavailable.";

            const string flaggedOrders = @"
                FROM dbo.[Order]
                WHERE LOWER(LTRIM(RTRIM(COALESCE(OrderStatus, ''))))
                    IN ('pending', 'unpaid', 'partial', 'processing')";

            AddChart("Flagged order value by status", Query(connection, @"
                SELECT LOWER(LTRIM(RTRIM(OrderStatus))), SUM(TotalAmount)" + flaggedOrders + @"
                GROUP BY LOWER(LTRIM(RTRIM(OrderStatus)))
                ORDER BY 1"), "bar", true);

            LoadDetails(connection, @"
                SELECT OrderID AS [Order ID], CustomerID AS [Customer ID],
                       OrderDate AS [Order date], TotalAmount AS [Full order value],
                       OrderStatus AS [Order status],
                       CASE WHEN OrderDate < DATEADD(day, 1, @today)
                            THEN DATEDIFF(day, OrderDate, @today) END AS [Days since order]" +
                flaggedOrders + " ORDER BY OrderDate, OrderID",
                DateParameter("@today", today));
        }

        private void LoadDeliveryExceptions(SqlConnection connection, DateTime today)
        {
            ReportNote = "Open deliveries become overdue the day after their expected delivery date. Orders already marked Delayed also appear.";

            const string classifiedOrders = @"
                WITH DeliveryExceptions AS (
                    SELECT OrderID, CustomerID, DeliveryDate, OrderStatus, DeliveryStatus,
                           CASE
                               WHEN DeliveryState = 'delayed' OR OrderState = 'delayed'
                                   OR (DeliveryDate < @today AND (
                                       DeliveryState IN ('pending', 'processing', 'shipped', 'in transit', 'out for delivery')
                                       OR (DeliveryState = '' AND OrderState IN ('pending', 'processing', 'confirmed', 'shipped'))
                                   )) THEN 'Delayed'
                               WHEN DeliveryState = 'pending'
                                   OR (DeliveryState = '' AND OrderState = 'pending') THEN 'Pending'
                           END AS Classification
                    FROM dbo.[Order]
                    CROSS APPLY (VALUES (
                        LOWER(LTRIM(RTRIM(COALESCE(OrderStatus, '')))),
                        LOWER(LTRIM(RTRIM(COALESCE(DeliveryStatus, ''))))
                    )) AS StatusValues(OrderState, DeliveryState)
                    WHERE OrderState NOT IN ('cancelled', 'canceled', 'refunded', 'returned', 'delivered')
                      AND DeliveryState NOT IN ('delivered', 'completed', 'cancelled', 'canceled', 'returned')
                )";

            AddChart("Pending and delayed orders", Query(connection, classifiedOrders + @"
                SELECT Classification, COUNT_BIG(*)
                FROM DeliveryExceptions
                WHERE Classification IS NOT NULL
                GROUP BY Classification
                ORDER BY Classification", DateParameter("@today", today)), "doughnut");

            LoadDetails(connection, classifiedOrders + @"
                SELECT OrderID AS [Order ID], CustomerID AS [Customer ID],
                       DeliveryDate AS [Expected delivery], OrderStatus AS [Order status],
                       DeliveryStatus AS [Delivery status], Classification AS [Report status],
                       CASE WHEN DeliveryDate < @today THEN DATEDIFF(day, DeliveryDate, @today)
                            ELSE NULL END AS [Days overdue]
                FROM DeliveryExceptions
                WHERE Classification IS NOT NULL
                ORDER BY CASE WHEN Classification = 'Delayed' THEN 0 ELSE 1 END,
                         CASE WHEN DeliveryDate IS NULL THEN 1 ELSE 0 END, DeliveryDate, OrderID",
                DateParameter("@today", today));
        }

        private void AddMonthlyAmountChart(string title, string amountColumn, string type)
        {
            var values = new DataTable();
            values.Columns.Add("Month", typeof(string));
            values.Columns.Add("Amount", typeof(decimal));

            foreach (DataRow row in Details.Rows)
                values.Rows.Add(row["Month"], row[amountColumn]);

            AddChart(title, values, type, true);
        }

        private void AddChart(string title, DataTable values, string type,
            bool currency = false, bool horizontal = false)
        {
            Charts.Add(new ReportChart
            {
                Title = title,
                Values = values,
                Type = type,
                Currency = currency,
                Horizontal = horizontal
            });
        }

        private void LoadDetails(SqlConnection connection, string sql,
            params SqlParameter[] parameters)
        {
            var paging = new List<SqlParameter>(parameters)
            {
                new SqlParameter("@skip", SqlDbType.BigInt) { Value = (PageNumber - 1L) * PageSize },
                new SqlParameter("@take", SqlDbType.Int) { Value = PageSize + 1 }
            };

            Details = Query(connection, sql + " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY",
                paging.ToArray());

            HasNextPage = Details.Rows.Count > PageSize;
            if (HasNextPage)
                Details.Rows.RemoveAt(PageSize);
        }

        private static SqlConnection OpenConnection()
        {
            var settings = ConfigurationManager.ConnectionStrings[ConnectionName];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                return null;

            var connection = new SqlConnection(settings.ConnectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }

        private static DataTable Query(SqlConnection connection, string sql,
            params SqlParameter[] parameters)
        {
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddRange(parameters);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }

        private static SqlParameter DateParameter(string name, DateTime value)
        {
            return new SqlParameter(name, SqlDbType.DateTime) { Value = value };
        }

        private static void LogSqlError(SqlException exception)
        {
            System.Diagnostics.Trace.TraceError("Reports SQL error number: {0}", exception.Number);
        }

        protected static string FormatCell(object value)
        {
            if (value == DBNull.Value)
                return "—";

            if (value is DateTime)
                return ((DateTime)value).ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture);

            if (value is decimal)
                return "R " + ((decimal)value).ToString("N2", CultureInfo.InvariantCulture);

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        protected static string RenderChart(ReportChart chart)
        {
            var labels = new List<string>();
            var values = new List<decimal>();

            foreach (DataRow row in chart.Values.Rows)
            {
                if (row.IsNull(1))
                    continue;

                labels.Add(FormatCell(row[0]));
                values.Add(Convert.ToDecimal(row[1], CultureInfo.InvariantCulture));
            }

            if (values.Count == 0)
                return "<p>No recorded values for this chart.</p>";

            string json = JsonConvert.SerializeObject(new
            {
                title = chart.Title,
                type = chart.Type,
                labels,
                values,
                currency = chart.Currency,
                horizontal = chart.Horizontal
            });

            return "<div class=\"br-chart-wrap\"><canvas class=\"br-chart\" role=\"img\" aria-label=\"" +
                   HttpUtility.HtmlAttributeEncode(chart.Title) + "\" data-chart=\"" +
                   HttpUtility.HtmlAttributeEncode(json) + "\"></canvas></div>";
        }
    }
}