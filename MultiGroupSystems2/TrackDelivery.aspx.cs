using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MultiGroupSystemsTester
{
    public partial class TrackDelivery : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !string.IsNullOrEmpty(Request.QueryString["orderId"]))
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

            if (string.IsNullOrEmpty(orderIdInput) || !int.TryParse(orderIdInput, out int orderId))
            {
                lblError.Text = "Please enter a valid numeric Order ID.";
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            // Join Order and Customer tables to build the full address string and retrieve DeliveryStatus
            string query = @"
        SELECT 
            o.DeliveryStatus, 
            ISNULL(c.StreetNumber, '') + ' ' + ISNULL(c.StreetName, '') + ', ' + ISNULL(c.Suburb, '') + ', ' + ISNULL(c.City, '') + ', ' + ISNULL(c.Province, '') AS FullAddress
        FROM [Order] o
        INNER JOIN Customer c ON o.CustomerID = c.CustomerID
        WHERE o.OrderID = @OrderID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["DeliveryStatus"] != DBNull.Value ? reader["DeliveryStatus"].ToString() : "Pending";
                                string address = reader["FullAddress"] != DBNull.Value ? reader["FullAddress"].ToString() : "";

                                lblStatusHeading.Text = status;

                                int stepIndex = 1;
                                switch (status.ToLower().Trim())
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
                                    default:
                                        stepIndex = 1;
                                        break;
                                }

                                string formattedAddress = address.Replace("'", "\\'");
                                string script = $"renderAddressPin('{formattedAddress}', {stepIndex});";
                                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateMapScript", script, true);
                            }
                            else
                            {
                                lblError.Text = "Order ID not found.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Database connection error: " + ex.Message;
                    }
                }
            }
        }
    }
}