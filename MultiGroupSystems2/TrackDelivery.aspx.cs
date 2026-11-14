
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace MultiGroupSystemsTester
{
    public partial class TrackDelivery : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack &&
                !string.IsNullOrWhiteSpace(Request.QueryString["orderId"]))
            {
                txtOrderId.Text = Request.QueryString["orderId"];
                TrackOrder(txtOrderId.Text.Trim());
            }
        }

        protected void btnTrack_Click(object sender, EventArgs e)
        {
            TrackOrder(txtOrderId.Text.Trim());
        }

        private void TrackOrder(string orderIdInput)
        {
            lblError.Text = "";
            lblStatusHeading.Text = "Ordered";

            // IMPORTANT:
            // Confirm this session key matches CustomerLogin.aspx.cs.
            object sessionCustomerId = Session["CustomerID"];

            int customerId;

            if (sessionCustomerId == null ||
    !int.TryParse(sessionCustomerId.ToString(), out customerId) ||
    customerId <= 0)
            {
                string returnUrl = "~/TrackDelivery.aspx";

                int requestedOrderId;
                if (int.TryParse(orderIdInput, out requestedOrderId) &&
                    requestedOrderId > 0)
                {
                    returnUrl += "?orderId=" + requestedOrderId;
                }

                string loginUrl = "~/Account/CustomerLogin.aspx?ReturnUrl=" +
                    Server.UrlEncode(returnUrl);

                Response.Redirect(ResolveUrl(loginUrl), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            int orderId;

            if (string.IsNullOrWhiteSpace(orderIdInput) ||
                !int.TryParse(orderIdInput, out orderId) ||
                orderId <= 0)
            {
                lblError.Text = "Please enter a valid Order ID.";
                return;
            }

            string connectionString =
                ConfigurationManager.ConnectionStrings[
                    "GroupWst14ConnectionString"].ConnectionString;

            const string query = @"
                SELECT
                    o.DeliveryStatus,
                    ISNULL(c.StreetNumber, '') + ' ' +
                    ISNULL(c.StreetName, '') + ', ' +
                    ISNULL(c.Suburb, '') + ', ' +
                    ISNULL(c.City, '') + ', ' +
                    ISNULL(c.Province, '') AS FullAddress
                FROM [Order] o
                INNER JOIN Customer c
                    ON o.CustomerID = c.CustomerID
                WHERE o.OrderID = @OrderID
                  AND o.CustomerID = @CustomerID";

            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connectionString))
                using (SqlCommand cmd =
                    new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int)
                        .Value = orderId;

                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = customerId;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblError.Text =
                                "Order not found or unavailable.";
                            return;
                        }

                        string status =
                            reader["DeliveryStatus"] == DBNull.Value
                            ? "Pending"
                            : reader["DeliveryStatus"].ToString();

                        string address =
                            reader["FullAddress"] == DBNull.Value
                            ? ""
                            : reader["FullAddress"].ToString();

                        lblStatusHeading.Text =
                            Server.HtmlEncode(status);

                        int stepIndex = 1;

                        switch (status.ToLowerInvariant().Trim())
                        {
                            case "pending":
                            case "not dispatched":
                            case "processing":
                                stepIndex = 1;
                                break;

                            case "ready for shipment":
                            case "shipped":
                            case "dispatched":
                                stepIndex = 2;
                                break;

                            case "out for delivery":
                                stepIndex = 3;
                                break;

                            case "delivered":
                            case "completed":
                                stepIndex = 4;
                                break;
                        }

                        // Safely encode the address as a JavaScript string.
                        JavaScriptSerializer serializer =
                            new JavaScriptSerializer();

                        string safeAddress =
                            serializer.Serialize(address);

                        string script =
                            "renderAddressPin(" +
                            safeAddress + ", " +
                            stepIndex + ");";

                        ScriptManager.RegisterStartupScript(
                            this,
                            GetType(),
                            "UpdateMapScript",
                            script,
                            true);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    "TrackDelivery error: " + ex);

                lblError.Text =
                    "Unable to retrieve tracking information. " +
                    "Please try again later.";
            }
        }
    }
}
