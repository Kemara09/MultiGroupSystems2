<%@ WebHandler Language="C#" Class="InvoiceDownload" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using MultiGroupSystemsTester.Models;
using MultiGroupSystemsTester.Services;

public class InvoiceDownload : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        int orderId;
        if (!int.TryParse(context.Request.QueryString["orderId"], out orderId))
        {
            context.Response.StatusCode = 400;
            context.Response.Write("Invalid order ID.");
            return;
        }

        string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
        var rows = new List<InvoiceReportRow>();

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();

            string custQuery = @"
                SELECT o.OrderID, o.OrderDate, o.OrderStatus, o.TotalAmount,
                       c.FirstName, c.LastName, c.EmailAddress, c.PhoneNumber,
                       c.StreetNumber, c.StreetName, c.Suburb, c.City, c.Province, c.PostalCode
                FROM [Order] o
                JOIN Customer c ON c.CustomerID = o.CustomerID
                WHERE o.OrderID = @OrderID";

            string custName, custEmail, custPhone, custAddress, orderStatus, orderDate;

            using (SqlCommand cmd = new SqlCommand(custQuery, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Write("Order not found.");
                        return;
                    }

                    custName = r["FirstName"] + " " + r["LastName"];
                    custEmail = r["EmailAddress"].ToString();
                    custPhone = r["PhoneNumber"].ToString();
                    custAddress = $"{r["StreetNumber"]} {r["StreetName"]}, {r["Suburb"]}, {r["City"]}, {r["Province"]}, {r["PostalCode"]}";
                    orderStatus = r["OrderStatus"].ToString();
                    orderDate = Convert.ToDateTime(r["OrderDate"]).ToString("dd MMMM yyyy");
                }
            }

            string lineQuery = @"
                SELECT p.ProductName, ol.Quantity, ol.Unit_Price, ol.Subtotal
                FROM Order_Line ol
                JOIN Inventory p ON p.ProductID = ol.ProductID
                WHERE ol.OrderID = @OrderID";

            decimal subtotalSum = 0;
            int lineNum = 1;

            using (SqlCommand cmd = new SqlCommand(lineQuery, conn))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        decimal unitPrice = Convert.ToDecimal(r["Unit_Price"]);
                        int qty = Convert.ToInt32(r["Quantity"]);
                        decimal lineSubtotal = Convert.ToDecimal(r["Subtotal"]);
                        decimal lineVat = lineSubtotal * 0.15m;
                        decimal lineTotal = lineSubtotal + lineVat;
                        subtotalSum += lineSubtotal;

                        rows.Add(new InvoiceReportRow
                        {
                            CompanyName = "MultiGroup Systems",
                            CompanyAddress = "123 Business Park, Durban, KZN, 4001",
                            CompanyEmail = "multigroup.systems@gmail.com",
                            CompanyPhone = "+27 31 000 0000",
                            VATNumber = "VAT Reg No: 4123456789",
                            InvoiceNumber = orderId,
                            InvoiceDate = orderDate,
                            InvoiceStatus = orderStatus,
                            CustomerName = custName,
                            CustomerEmail = custEmail,
                            CustomerPhone = custPhone,
                            CustomerAddress = custAddress,
                            StaffName = "Online Order",
                            LineNumber = lineNum++,
                            ProductName = r["ProductName"].ToString(),
                            UnitPriceStr = "R " + unitPrice.ToString("N2"),
                            Quantity = qty,
                            LineSubtotalStr = "R " + lineSubtotal.ToString("N2"),
                            LineVATStr = "R " + lineVat.ToString("N2"),
                            LineTotalStr = "R " + lineTotal.ToString("N2")
                        });
                    }
                }
            }

            if (rows.Count == 0)
            {
                context.Response.StatusCode = 404;
                context.Response.Write("No line items found for this order.");
                return;
            }

            decimal vatSum = subtotalSum * 0.15m;
            decimal grandTotal = subtotalSum + vatSum;
            string subtotalStr = "R " + subtotalSum.ToString("N2");
            string vatStr = "R " + vatSum.ToString("N2");
            string grandTotalStr = "R " + grandTotal.ToString("N2");

            foreach (var row in rows)
            {
                row.SubtotalStr = subtotalStr;
                row.VATAmountStr = vatStr;
                row.GrandTotalStr = grandTotalStr;
            }
        }

        InvoiceService.SendInvoiceToResponse(rows, orderId, context.Response);
    }

    public bool IsReusable => false;
}