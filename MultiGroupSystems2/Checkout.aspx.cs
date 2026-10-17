using MultiGroupSystemsTester.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MultiGroupSystemsTester
{
    public partial class Checkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect(
                    "~/Account/CustomerLogin.aspx?ReturnUrl=" +
                    Server.UrlEncode("~/Checkout.aspx")
                );

                return;
            }

            if (!IsPostBack)
            {
                PrefillFromCustomer();
                LoadCheckoutSummary();
                UpdateAddressDisplay();
            }
        }


        private void PrefillFromCustomer()
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = "SELECT FirstName, LastName, PhoneNumber, EmailAddress, StreetNumber, StreetName, Suburb, City, Province, PostalCode FROM Customer WHERE CustomerID = @CustomerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", Session["CustomerID"]);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Hidden fields (read later by the invoice code)
                            txtFirstName.Text = reader["FirstName"].ToString();
                            txtLastName.Text = reader["LastName"].ToString();
                            txtEmail.Text = reader["EmailAddress"].ToString();
                            txtStreetNumber.Text = reader["StreetNumber"].ToString();
                            txtStreetName.Text = reader["StreetName"].ToString();

                            // Visible fields in the CHANGE panel
                            txtPhone.Text = reader["PhoneNumber"].ToString();
                            txtSuburb.Text = reader["Suburb"].ToString();
                            txtCity.Text = reader["City"].ToString();
                            txtPostalCode.Text = reader["PostalCode"].ToString();

                            // The panel shows one combined street field
                            txtStreetAddress.Text =
                                (txtStreetNumber.Text.Trim() + " " + txtStreetName.Text.Trim()).Trim();

                            // FindByValue avoids an exception if the saved province
                            // doesn't exactly match a list item
                            var province = ddlProvince.Items.FindByValue(reader["Province"].ToString().Trim());
                            if (province != null)
                            {
                                ddlProvince.ClearSelection();
                                province.Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PREFILL ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Splits the single "Street address" box into street number and street name,
        /// so the hidden fields used by the invoice code stay in step.
        /// </summary>
        private void SyncStreetFields()
        {
            string streetAddress = txtStreetAddress.Text.Trim();

            string streetNumber = "";
            string streetName = "";

            string[] addressParts = streetAddress.Split(
                new[] { ' ' },
                2,
                StringSplitOptions.RemoveEmptyEntries
            );

            if (addressParts.Length > 0)
            {
                streetNumber = addressParts[0];
            }

            if (addressParts.Length > 1)
            {
                streetName = addressParts[1].Trim();
            }

            txtStreetNumber.Text = streetNumber;
            txtStreetName.Text = streetName;
        }

        /// <summary>
        /// Returns DBNull for empty text, otherwise the trimmed text.
        /// </summary>
        private static object DbText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }

        private void LoadCheckoutSummary()
        {
            List<CartItem> cart = Session["Cart"] as List<CartItem>;

            if (cart == null || cart.Count == 0)
            {
                lblItemCount.Text = "0";
                lblSubtotal.Text = "0.00";
                lblVAT.Text = "0.00";
                lblGrandTotal.Text = "0.00";

                rptOrderItems.DataSource = null;
                rptOrderItems.DataBind();

                return;
            }

            decimal subtotal = cart.Sum(c => c.Subtotal);
            decimal vat = subtotal * 0.15m;
            decimal grandTotal = subtotal + vat;

            int itemCount = cart.Sum(c => c.Quantity);

            lblItemCount.Text = itemCount.ToString();
            lblSubtotal.Text = subtotal.ToString("N2");
            lblVAT.Text = vat.ToString("N2");
            lblGrandTotal.Text = grandTotal.ToString("N2");

            rptOrderItems.DataSource = cart;
            rptOrderItems.DataBind();
        }

        private void UpdateAddressDisplay()
        {
            lblAddressName.Text =
                $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";

            lblAddress.Text =
                $"{txtStreetNumber.Text.Trim()} {txtStreetName.Text.Trim()}<br />" +
                $"{txtSuburb.Text.Trim()}<br />" +
                $"{txtCity.Text.Trim()}, {ddlProvince.SelectedValue}<br />" +
                $"{txtPostalCode.Text.Trim()}";

            lblAddressPhone.Text = txtPhone.Text.Trim();
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            // The page posts back here. The CHANGE panel closes again after a postback,
            // so make sure the address card shows any edits the user already made.
            SyncStreetFields();
            UpdateAddressDisplay();

            pnlCardDetails.Visible = ddlPaymentMethod.SelectedValue == "Card";
            pnlEFTDetails.Visible = ddlPaymentMethod.SelectedValue == "EFT";
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            SyncStreetFields();
            UpdateAddressDisplay();

            if (!Page.IsValid) return;

            List<CartItem> cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                lblError.Text = "Your cart is empty.";
                lblError.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(ddlPaymentMethod.SelectedValue))
            {
                lblError.Text = "Please select a payment method.";
                lblError.Visible = true;
                return;
            }

            // The date comes from a hidden field in the browser, so validate it:
            // it must be one of the four dates offered (today to today + 3 days).
            DateTime deliveryDate;
            if (!DateTime.TryParseExact(hfDeliveryDate.Value, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out deliveryDate)
                || deliveryDate.Date < DateTime.Today
                || deliveryDate.Date > DateTime.Today.AddDays(3))
            {
                lblError.Text = "Please choose a valid delivery date.";
                lblError.Visible = true;
                return;
            }

            // Delivery instructions: only used if the box was ticked and something was typed.
            // Trimmed to 250 characters to match the database column.
            string deliveryInstructions = "";
            if (chkDeliveryInstructions.Checked && !string.IsNullOrWhiteSpace(txtDeliveryInstructions.Text))
            {
                deliveryInstructions = txtDeliveryInstructions.Text.Trim();
                if (deliveryInstructions.Length > 250)
                {
                    deliveryInstructions = deliveryInstructions.Substring(0, 250);
                }
            }

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlTransaction tx = conn.BeginTransaction();
                    try
                    {
                        int customerId = Convert.ToInt32(Session["CustomerID"]);

                        decimal totalAmount = cart.Sum(c => c.Subtotal);
                        int orderId;

                        // Collection orders don't need a delivery address
                        bool isDelivery = hfDeliveryMethod.Value != "Collection";

                        const string insertOrder = @"
                            INSERT INTO [Order] (CustomerID, StaffID, TotalAmount, OrderDate, OrderStatus, DeliveryStatus, DeliveryDate, PaymentMethod,
                                                 DeliveryStreetAddress, DeliverySuburb, DeliveryProvince, DeliveryPostalCode, DeliveryPhone,
                                                 DeliveryInstructions)
                            VALUES (@CustomerID, NULL, @TotalAmount, @OrderDate, @OrderStatus, @DeliveryStatus, @DeliveryDate, @PaymentMethod,
                                    @DelStreet, @DelSuburb, @DelProvince, @DelPostal, @DelPhone,
                                    @DelInstructions);
                            SELECT SCOPE_IDENTITY();";
                        using (SqlCommand cmdOrder = new SqlCommand(insertOrder, conn, tx))
                        {
                            cmdOrder.Parameters.AddWithValue("@CustomerID", customerId);
                            cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            cmdOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                            cmdOrder.Parameters.AddWithValue("@OrderStatus", "Pending");
                            cmdOrder.Parameters.AddWithValue("@DeliveryStatus", "Not Dispatched");
                            cmdOrder.Parameters.AddWithValue("@DeliveryDate", deliveryDate.Date);
                            cmdOrder.Parameters.AddWithValue("@PaymentMethod", ddlPaymentMethod.SelectedValue);

                            // Address used for THIS order only (the Customer table is never changed)
                            cmdOrder.Parameters.AddWithValue("@DelStreet", isDelivery ? DbText(txtStreetAddress.Text) : DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@DelSuburb", isDelivery ? DbText(txtSuburb.Text) : DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@DelProvince", isDelivery ? DbText(ddlProvince.SelectedValue) : DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@DelPostal", isDelivery ? DbText(txtPostalCode.Text) : DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@DelPhone", isDelivery ? DbText(txtPhone.Text) : DBNull.Value);

                            // Customer's delivery note (NULL if they didn't add one)
                            cmdOrder.Parameters.AddWithValue("@DelInstructions", DbText(deliveryInstructions));

                            orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());
                        }

                        foreach (CartItem item in cart)
                        {
                            const string insertLine = @"
                                INSERT INTO Order_Line (ProductID, OrderID, Quantity, Unit_Price, Subtotal)
                                VALUES (@ProductID, @OrderID, @Quantity, @UnitPrice, @Subtotal)";
                            using (SqlCommand cmdLine = new SqlCommand(insertLine, conn, tx))
                            {
                                cmdLine.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmdLine.Parameters.AddWithValue("@OrderID", orderId);
                                cmdLine.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmdLine.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                                cmdLine.Parameters.AddWithValue("@Subtotal", item.Subtotal);
                                cmdLine.ExecuteNonQuery();
                            }

                            const string updateStock = "UPDATE Inventory SET QuantityInStock = QuantityInStock - @Quantity WHERE ProductID = @ProductID";
                            using (SqlCommand cmdStock = new SqlCommand(updateStock, conn, tx))
                            {
                                cmdStock.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmdStock.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmdStock.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        Session["Cart"] = null;


                        var invoiceRows = new List<MultiGroupSystemsTester.Models.InvoiceReportRow>();
                        decimal subtotalSum = cart.Sum(c => c.Subtotal);
                        decimal vatSum = subtotalSum * 0.15m;
                        decimal grandTotal = subtotalSum + vatSum;
                        int lineNum = 1;

                        foreach (CartItem item in cart)
                        {
                            decimal lineVat = item.Subtotal * 0.15m;
                            decimal lineTotal = item.Subtotal + lineVat;

                            invoiceRows.Add(new MultiGroupSystemsTester.Models.InvoiceReportRow
                            {
                                CompanyName = "Multigroup Inc.",
                                CompanyAddress = "123 Business Park, Durban, KZN, 4001",
                                CompanyEmail = "multigroup.systems@gmail.com",
                                CompanyPhone = "+27 31 000 0000",
                                VATNumber = "VAT Reg No: 4123456789",
                                InvoiceNumber = orderId,
                                InvoiceDate = DateTime.Now.ToString("dd MMMM yyyy"),
                                InvoiceStatus = "Pending",
                                CustomerName = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}",
                                CustomerEmail = txtEmail.Text.Trim(),
                                CustomerPhone = txtPhone.Text.Trim(),
                                CustomerAddress = $"{txtStreetNumber.Text.Trim()} {txtStreetName.Text.Trim()}, {txtSuburb.Text.Trim()}, {txtCity.Text.Trim()}, {ddlProvince.SelectedValue}, {txtPostalCode.Text.Trim()}",
                                StaffName = "Online Order",
                                LineNumber = lineNum++,
                                ProductName = item.ProductName,
                                UnitPriceStr = "R " + item.UnitPrice.ToString("N2"),
                                Quantity = item.Quantity,
                                LineSubtotalStr = "R " + item.Subtotal.ToString("N2"),
                                LineVATStr = "R " + lineVat.ToString("N2"),
                                LineTotalStr = "R " + lineTotal.ToString("N2"),
                                SubtotalStr = "R " + subtotalSum.ToString("N2"),
                                VATAmountStr = "R " + vatSum.ToString("N2"),
                                GrandTotalStr = "R " + grandTotal.ToString("N2")
                            });
                        }

                        byte[] invoicePdf = MultiGroupSystemsTester.Services.InvoiceService.GenerateInvoicePdf(invoiceRows);

                        bool emailSent = false;
                        try
                        {
                            emailSent = MultiGroupSystemsTester.Services.EmailService.SendInvoiceEmail(
                                txtEmail.Text.Trim(), txtFirstName.Text.Trim(), orderId, invoicePdf);
                        }
                        catch (Exception emailEx)
                        {
                            System.Diagnostics.Debug.WriteLine("EMAIL ERROR: " + emailEx.Message);
                        }

                        bool smsSent = false;
                        try
                        {
                            string smsNumber = "+27" + txtPhone.Text.Trim().TrimStart('0');
                            string smsMessage = $"Multigroup Inc.\nOrder #{orderId} confirmed\nTotal: {invoiceRows[0].GrandTotalStr}\nThank you, {txtFirstName.Text.Trim()}!";
                            smsSent = MultiGroupSystemsTester.Services.SmsService.SendSms(smsNumber, smsMessage);
                        }
                        catch (Exception smsEx)
                        {
                            System.Diagnostics.Debug.WriteLine("SMS ERROR: " + smsEx.Message);
                        }

                        string invoiceUrl = Page.ResolveUrl("~/InvoiceDownload.ashx") + "?orderId=" + orderId;
                        string emailNote = emailSent ? " We've also emailed a copy to you" : "";
                        string smsNote = smsSent ? " and texted you a confirmation." : (emailSent ? "." : "");

                        // Delivery / collection details chosen on the page
                        string method = isDelivery ? "Delivery" : "Collection";
                        string deliveryNote = $" {method} date: {deliveryDate:dddd, dd MMMM yyyy}.";

                        string instructionsNote = "";
                        if (deliveryInstructions.Length > 0)
                        {
                            instructionsNote = "<p>Delivery instructions: " +
                                System.Web.HttpUtility.HtmlEncode(deliveryInstructions) +
                                "</p>";
                        }

                        var summary = new System.Text.StringBuilder();
                        summary.Append("<table class='table table-sm mt-3 mb-2'>");
                        summary.Append("<thead><tr><th>Product</th><th class='text-end'>Qty</th><th class='text-end'>Unit Price</th><th class='text-end'>Line Total</th></tr></thead><tbody>");
                        foreach (var row in invoiceRows)
                        {
                            summary.Append("<tr>");
                            summary.Append($"<td>{System.Web.HttpUtility.HtmlEncode(row.ProductName)}</td>");
                            summary.Append($"<td class='text-end'>{row.Quantity}</td>");
                            summary.Append($"<td class='text-end'>{row.UnitPriceStr}</td>");
                            summary.Append($"<td class='text-end'>{row.LineTotalStr}</td>");
                            summary.Append("</tr>");
                        }
                        summary.Append("</tbody></table>");
                        summary.Append("<div class='text-end small'>");
                        summary.Append($"<div>Subtotal: {invoiceRows[0].SubtotalStr}</div>");
                        summary.Append($"<div>VAT: {invoiceRows[0].VATAmountStr}</div>");
                        summary.Append($"<div class='fw-bold'>Total: {invoiceRows[0].GrandTotalStr}</div>");
                        summary.Append("</div>");

                        pnlSuccess.Controls.Add(new System.Web.UI.LiteralControl(
                            $"<p>Thank you! Your order #{orderId} has been placed.{emailNote}{smsNote}{deliveryNote} " +
                            $"<a href='{invoiceUrl}' target='_blank'>View your invoice</a></p>" +
                            instructionsNote +
                            summary.ToString()));
                        pnlSuccess.Visible = true;
                        pnlForm.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();

                        // Full error goes to the Visual Studio Output window (not shown to customers)
                        System.Diagnostics.Debug.WriteLine("ORDER ERROR: " + ex);

                        lblError.Text = "Something went wrong placing your order. Please try again.";
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Could not connect to place your order right now.";
                lblError.Visible = true;
            }
        }
    }
}