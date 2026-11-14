using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace MultiGroupSystemsTester.Admin
{
    public partial class OrderLogistics : AdminPage
    {
        private static readonly string ConnString =
            ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

        private static readonly string[] TimelineSteps = { "Order Placed", "Processing", "In Transit", "Delivered" };

        // Known statuses in workflow order. Any other value found in the table is added automatically.
        private static readonly string[] OrderStatusOptions =
            { "Pending", "Processing", "Ready For Shipment", "In Transit", "Delivered", "Completed", "On Hold", "Cancelled" };

        private static readonly string[] DeliveryStatusOptions =
            { "Not Dispatched", "Pending", "Processing", "In Transit", "Delayed", "Delivered", "Failed" };

        // The order comes from the query string (?orderId=4038), which also survives postbacks
        private int OrderId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["orderId"], out id) ? id : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (OrderId <= 0)
            {
                // No order chosen: send the admin to the list, which has the search box
                Response.Redirect("~/Admin/ViewOrders.aspx");
                return;
            }

            try
            {
                LoadStatusOptions();

                if (!BindOrder())
                    ShowNotFound("Order " + OrderId + " could not be found.");
            }
            catch (Exception ex)
            {
                ShowNotFound("Could not load the order: " + ex.Message);
            }
        }

        // ---------- STATUS DROPDOWNS ----------

        private void LoadStatusOptions()
        {
            FillStatusList(ddlOrderStatus, OrderStatusOptions, "OrderStatus");
            FillStatusList(ddlDeliveryStatus, DeliveryStatusOptions, "DeliveryStatus");
        }

        // 'column' is always one of two fixed names from this file, never user input
        private static void FillStatusList(DropDownList ddl, string[] baseOptions, string column)
        {
            ddl.Items.Clear();
            foreach (string option in baseOptions)
                ddl.Items.Add(option);

            // Add any other value already used in the table, with its exact spelling
            string sql = "SELECT DISTINCT " + column + " FROM dbo.[Order] WHERE " + column + " IS NOT NULL ORDER BY " + column;

            using (var conn = new SqlConnection(ConnString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string value = reader.GetString(0).Trim();
                        if (value.Length == 0) continue;

                        bool exists = false;
                        foreach (ListItem item in ddl.Items)
                        {
                            if (string.Equals(item.Value, value, StringComparison.OrdinalIgnoreCase))
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists) ddl.Items.Add(value);
                    }
                }
            }
        }

        // ---------- LOAD ----------

        private bool BindOrder()
        {
            const string sql = @"
                SELECT o.OrderID, o.OrderDate, o.OrderStatus, o.DeliveryStatus, o.DeliveryDate,
                       o.TotalAmount, o.PaymentMethod,
                       o.DeliveryStreetAddress, o.DeliverySuburb, o.DeliveryProvince,
                       o.DeliveryPostalCode, o.DeliveryPhone, o.DeliveryInstructions,
                       c.FirstName, c.LastName
                FROM dbo.[Order] o
                LEFT JOIN dbo.Customer c ON c.CustomerID = o.CustomerID
                WHERE o.OrderID = @OrderID";

            using (var conn = new SqlConnection(ConnString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderId;
                conn.Open();

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return false;

                    DateTime? orderDate = Dt(r, "OrderDate");
                    DateTime? deliveryDate = Dt(r, "DeliveryDate");
                    string orderStatus = Str(r, "OrderStatus");
                    string deliveryStatus = Str(r, "DeliveryStatus");
                    string customer = (Str(r, "FirstName") + " " + Str(r, "LastName")).Trim();
                    string total = r["TotalAmount"] == DBNull.Value
                        ? "Not set"
                        : "R " + Convert.ToDecimal(r["TotalAmount"]).ToString("N2", CultureInfo.InvariantCulture);

                    // Full address on one block: street, suburb, province, postal code
                    var addressParts = new[]
                    {
                        Str(r, "DeliveryStreetAddress"), Str(r, "DeliverySuburb"),
                        Str(r, "DeliveryProvince"), Str(r, "DeliveryPostalCode")
                    };
                    string address = string.Join(", ", Array.FindAll(addressParts, s => s.Length > 0));

                    // Read-only details
                    txtOrderNumber.Text = FormatOrderNumber(OrderId);
                    txtCustomerName.Text = customer.Length > 0 ? customer : "Unknown customer";
                    txtOrderDate.Text = orderDate.HasValue ? orderDate.Value.ToString("yyyy-MM-dd") : "";
                    txtPayment.Text = Str(r, "PaymentMethod");
                    txtAddress.Text = address;
                    txtPhone.Text = Str(r, "DeliveryPhone");
                    txtDeliveryDate.Text = deliveryDate.HasValue ? deliveryDate.Value.ToString("yyyy-MM-dd") : "";
                    txtCustomerInstructions.Text = Str(r, "DeliveryInstructions");

                    // The two editable fields
                    SelectOrAdd(ddlOrderStatus, orderStatus);
                    SelectOrAdd(ddlDeliveryStatus, deliveryStatus);

                    // Header, summary cards and timeline
                    litStatusBadge.Text = orderStatus.Length > 0 ? orderStatus : "Unknown";
                    statusBadge.Attributes["class"] = "status-badge " + StatusClass(orderStatus);
                    litSumOrder.Text = FormatOrderNumber(OrderId);
                    litSumStatus.Text = deliveryStatus.Length > 0 ? deliveryStatus : "Not set";
                    litSumDate.Text = deliveryDate.HasValue
                        ? deliveryDate.Value.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)
                        : "Not set";
                    litSumTotal.Text = total;

                    ltTimeline.Text = BuildTimeline(deliveryStatus, orderDate, deliveryDate);
                }
            }
            return true;
        }

        // ---------- SAVE ----------

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (OrderId <= 0)
            {
                Response.Redirect("~/Admin/ViewOrders.aspx");
                return;
            }

            try
            {
                const string sql = @"UPDATE dbo.[Order]
                                     SET OrderStatus = @OrderStatus, DeliveryStatus = @DeliveryStatus
                                     WHERE OrderID = @OrderID";

                using (var conn = new SqlConnection(ConnString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@OrderStatus", SqlDbType.VarChar, 50).Value = ddlOrderStatus.SelectedValue;
                    cmd.Parameters.Add("@DeliveryStatus", SqlDbType.VarChar, 50).Value = ddlDeliveryStatus.SelectedValue;
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderId;
                    conn.Open();

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        ShowNotFound("Order " + OrderId + " could not be found.");
                        return;
                    }
                }

                BindOrder(); // refresh the badge, summary cards and timeline
                ShowMessage("Order status set to \"" + ddlOrderStatus.SelectedValue +
                            "\" and delivery status set to \"" + ddlDeliveryStatus.SelectedValue + "\".", false);
            }
            catch (Exception ex)
            {
                ShowMessage("Could not update the status: " + ex.Message, true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Dashboard.aspx");
        }

        // ---------- TIMELINE (follows the DELIVERY status) ----------

        private static string BuildTimeline(string deliveryStatus, DateTime? orderDate, DateTime? deliveryDate)
        {
            string s = (deliveryStatus ?? "").Trim().ToLowerInvariant();
            string placedDate = orderDate.HasValue
                ? orderDate.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : "Placed";

            // Delayed / Failed stop the normal flow, so show them as their own step
            bool delayed = s.Contains("delay");
            bool failed = s == "failed";
            if (delayed || failed)
            {
                return StepHtml("progress-step completed", "✓", TimelineSteps[0], placedDate)
                     + LineHtml(true)
                     + StepHtml(failed ? "progress-step stopped-step cancelled-step" : "progress-step stopped-step",
                                failed ? "✕" : "❚❚", deliveryStatus.Trim(), "Current Status");
            }

            // 0 = only placed, 1 = Processing, 2 = In Transit, 3 = Delivered
            int current;
            switch (s)
            {
                case "processing": current = 1; break;
                case "in transit": current = 2; break;
                case "delivered": current = 3; break;
                default: current = 0; break; // Not Dispatched, Pending or unknown
            }

            var sb = new StringBuilder();
            for (int i = 0; i < TimelineSteps.Length; i++)
            {
                bool completed = (i == 0) || (i < current) || (current == 3);
                bool active = !completed && i == current;

                string cssClass = completed ? "progress-step completed" : (active ? "progress-step active-step" : "progress-step");
                string icon = completed ? "✓" : (active ? (i == 2 ? "🚚" : "●") : (i + 1).ToString());

                string sub;
                if (i == 0) sub = placedDate;
                else if (completed) sub = "Completed";
                else if (active) sub = "Current Status";
                else if (i == 3 && deliveryDate.HasValue)
                    sub = "Expected " + deliveryDate.Value.ToString("dd MMM", CultureInfo.InvariantCulture);
                else sub = "Pending";

                sb.Append(StepHtml(cssClass, icon, TimelineSteps[i], sub));
                if (i < TimelineSteps.Length - 1) sb.Append(LineHtml(completed));
            }
            return sb.ToString();
        }

        private static string StepHtml(string cssClass, string icon, string title, string sub)
        {
            return "<div class=\"" + cssClass + "\">"
                 + "<div class=\"progress-icon\">" + icon + "</div>"
                 + "<div><strong>" + HttpUtility.HtmlEncode(title) + "</strong>"
                 + "<small>" + HttpUtility.HtmlEncode(sub) + "</small></div></div>";
        }

        private static string LineHtml(bool completed)
        {
            return "<div class=\"progress-line" + (completed ? " completed-line" : "") + "\"></div>";
        }

        // ---------- HELPERS ----------

        private void ShowMessage(string text, bool isError)
        {
            msgBox.Visible = true;
            msgBox.InnerText = text; // InnerText HTML-encodes the message
            msgBox.Attributes["class"] = "lg-message " + (isError ? "lg-error" : "lg-success");
        }

        private void ShowNotFound(string text)
        {
            phContent.Visible = false;
            ShowMessage(text, true);
        }

        private static string FormatOrderNumber(int id)
        {
            return "ORD-" + id;
        }

        // Picks a colour class for the order status badge
        private static string StatusClass(string status)
        {
            switch ((status ?? "").Trim().ToLowerInvariant())
            {
                case "pending": return "st-pending";
                case "processing": return "st-processing";
                case "ready for shipment": return "st-ready";
                case "in transit": return "st-transit";
                case "delivered":
                case "completed": return "st-delivered";
                case "on hold": return "st-hold";
                case "cancelled":
                case "canceled": return "st-cancelled";
                default: return "st-other";
            }
        }

        private static string Str(IDataRecord r, string col)
        {
            object v = r[col];
            return v == DBNull.Value ? "" : Convert.ToString(v).Trim();
        }

        private static DateTime? Dt(IDataRecord r, string col)
        {
            object v = r[col];
            return v == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(v);
        }

        // Select the matching item; if the value is not in the list, add it so it is not lost
        private static void SelectOrAdd(DropDownList ddl, string value)
        {
            ddl.ClearSelection();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (ddl.Items.Count > 0) ddl.SelectedIndex = 0;
                return;
            }

            ListItem item = ddl.Items.FindByValue(value);
            if (item == null)
            {
                item = new ListItem(value, value);
                ddl.Items.Insert(0, item);
            }
            item.Selected = true;
        }
    }
}