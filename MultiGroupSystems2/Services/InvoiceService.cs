using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using iText.Html2pdf;
using MultiGroupSystemsTester.Models;

namespace MultiGroupSystemsTester.Services
{
    public static class InvoiceService
    {
        public static byte[] GenerateInvoicePdf(List<InvoiceReportRow> rows)
        {
            if (rows == null || rows.Count == 0)
                throw new ArgumentException("No invoice rows provided.", nameof(rows));

            string html = BuildInvoiceHtml(rows);

            using (var pdfStream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(html, pdfStream);
                return pdfStream.ToArray();
            }
        }

        public static void SendInvoiceToResponse(List<InvoiceReportRow> rows, int orderId, HttpResponse response)
        {
            if (rows == null || rows.Count == 0)
            {
                response.StatusCode = 404;
                response.Write("No invoice data found.");
                return;
            }

            byte[] pdfBytes = GenerateInvoicePdf(rows);

            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", $"inline; filename=Invoice_{orderId}.pdf");
            response.BinaryWrite(pdfBytes);
            response.End();
        }

        private static string BuildInvoiceHtml(List<InvoiceReportRow> rows)
        {
            var header = rows[0]; // company/customer/totals info is repeated on every row

            var itemsHtml = new StringBuilder();
            foreach (var row in rows)
            {
                itemsHtml.Append($@"
                    <tr>
                        <td>{row.LineNumber}</td>
                        <td>{row.ProductName}</td>
                        <td style='text-align:center;'>{row.Quantity}</td>
                        <td style='text-align:right;'>{row.UnitPriceStr}</td>
                        <td style='text-align:right;'>{row.LineSubtotalStr}</td>
                        <td style='text-align:right;'>{row.LineVATStr}</td>
                        <td style='text-align:right;'>{row.LineTotalStr}</td>
                    </tr>");
            }

            return $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; color: #333; margin: 40px; }}
                    .header {{ background-color: #0C2A44; color: white; padding: 20px; }}
                    .header h1 {{ margin: 0; font-size: 22px; }}
                    .header p {{ margin: 4px 0 0 0; font-size: 12px; color: #cfd8e0; }}
                    .company-meta {{ margin-top: 10px; font-size: 11px; color: #cfd8e0; }}
                    .invoice-meta {{ margin: 20px 0; width: 100%; }}
                    .invoice-meta td {{ padding: 2px 0; font-size: 13px; vertical-align: top; }}
                    .invoice-meta .col {{ width: 50%; }}
                    table.items {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
                    table.items th {{ background-color: #0F6E56; color: white; padding: 8px; text-align: left; font-size: 12px; }}
                    table.items td {{ padding: 8px; border-bottom: 1px solid #ddd; font-size: 12px; }}
                    .totals {{ width: 100%; margin-top: 15px; }}
                    .totals td {{ padding: 4px 8px; font-size: 13px; }}
                    .totals .grand-total {{ font-weight: bold; font-size: 15px; color: #0C2A44; border-top: 2px solid #0C2A44; }}
                    .footer {{ margin-top: 40px; font-size: 11px; color: #888; }}
                </style>
            </head>
            <body>
                <div class='header'>
                    <h1>{header.CompanyName}</h1>
                    <p>Invoice #{header.InvoiceNumber}</p>
                    <div class='company-meta'>
                        {header.CompanyAddress}<br/>
                        {header.CompanyEmail} | {header.CompanyPhone}<br/>
                        {header.VATNumber}
                    </div>
                </div>

                <table class='invoice-meta'>
                    <tr>
                        <td class='col'>
                            <strong>Bill To:</strong><br/>
                            {header.CustomerName}<br/>
                            {header.CustomerAddress}<br/>
                            {header.CustomerEmail}<br/>
                            {header.CustomerPhone}
                        </td>
                        <td class='col'>
                            <strong>Invoice Date:</strong> {header.InvoiceDate}<br/>
                            <strong>Status:</strong> {header.InvoiceStatus}<br/>
                            <strong>Handled By:</strong> {header.StaffName}
                        </td>
                    </tr>
                </table>

                <table class='items'>
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Product</th>
                            <th style='text-align:center;'>Qty</th>
                            <th style='text-align:right;'>Unit Price</th>
                            <th style='text-align:right;'>Subtotal</th>
                            <th style='text-align:right;'>VAT</th>
                            <th style='text-align:right;'>Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        {itemsHtml}
                    </tbody>
                </table>

                <table class='totals'>
                    <tr><td style='text-align:right;'>Subtotal:</td><td style='text-align:right; width:100px;'>{header.SubtotalStr}</td></tr>
                    <tr><td style='text-align:right;'>VAT (15%):</td><td style='text-align:right;'>{header.VATAmountStr}</td></tr>
                    <tr class='grand-total'><td style='text-align:right;'>Grand Total:</td><td style='text-align:right;'>{header.GrandTotalStr}</td></tr>
                </table>

                <div class='footer'>
                    Thank you for shopping with {header.CompanyName}. This is a computer-generated invoice.
                </div>
            </body>
            </html>";
        }
    }
}