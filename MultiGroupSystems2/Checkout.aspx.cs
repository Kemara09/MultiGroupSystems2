using MultiGroupSystemsTester.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
                Response.Redirect("~/Account/CustomerLogin.aspx?ReturnUrl=" + Server.UrlEncode("~/Checkout.aspx"));
                return;
            }

            if (!IsPostBack)
            {
                if (!IsPostBack && Session["CustomerID"] != null)
                    PrefillFromCustomer();
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
                            txtFirstName.Text = reader["FirstName"].ToString();
                            txtLastName.Text = reader["LastName"].ToString();
                            txtPhone.Text = reader["PhoneNumber"].ToString();
                            txtEmail.Text = reader["EmailAddress"].ToString();
                            txtStreetNumber.Text = reader["StreetNumber"].ToString();
                            txtStreetName.Text = reader["StreetName"].ToString();
                            txtSuburb.Text = reader["Suburb"].ToString();
                            txtCity.Text = reader["City"].ToString();
                            ddlProvince.SelectedValue = reader["Province"].ToString();
                            txtPostalCode.Text = reader["PostalCode"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlCardDetails.Visible = ddlPaymentMethod.SelectedValue == "Card";
            pnlEFTDetails.Visible = ddlPaymentMethod.SelectedValue == "EFT";
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
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

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlTransaction tx = conn.BeginTransaction();
                    try
                    {
                        int customerId;

                        
                            customerId = Convert.ToInt32(Session["CustomerID"]);
                        
                        
                        
                            

                        decimal totalAmount = cart.Sum(c => c.Subtotal);
                        int orderId;
                        const string insertOrder = @"
                            INSERT INTO [Order] (CustomerID, StaffID, TotalAmount, OrderDate, OrderStatus, DeliveryStatus, PaymentMethod)
                            VALUES (@CustomerID, NULL, @TotalAmount, @OrderDate, @OrderStatus, @DeliveryStatus, @PaymentMethod);
                            SELECT SCOPE_IDENTITY();";
                        using (SqlCommand cmdOrder = new SqlCommand(insertOrder, conn, tx))
                        {
                            cmdOrder.Parameters.AddWithValue("@CustomerID", customerId);
                            cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            cmdOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                            cmdOrder.Parameters.AddWithValue("@OrderStatus", "Pending");
                            cmdOrder.Parameters.AddWithValue("@DeliveryStatus", "Not Dispatched");
                            cmdOrder.Parameters.AddWithValue("@PaymentMethod", ddlPaymentMethod.SelectedValue);
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
                        string smsNote = smsSent ? " and texted you a confirmation." : "";

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
                            $"<p>Thank you! Your order #{orderId} has been placed.{emailNote}{smsNote} " +
                            $"<a href='{invoiceUrl}' target='_blank'>View your invoice</a></p>" +
                            summary.ToString()));
                        pnlSuccess.Visible = true;
                        pnlForm.Visible = false;
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
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