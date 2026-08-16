using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using MultiGroupSystemsTester.Models;

namespace MultiGroupSystemsTester.Services
{
    public static class InvoiceService
    {
        /// <summary>
        /// Renders the invoice to a PDF byte array using the same InvoiceReport.rpt
        /// and row structure your M2 WinForms system uses.
        /// </summary>
        public static byte[] GenerateInvoicePdf(List<InvoiceReportRow> rows)
        {
            string reportPath = HttpContext.Current.Server.MapPath("~/App_Data/InvoiceReport.rpt");

            using (ReportDocument report = new ReportDocument())
            {
                report.Load(reportPath);
                report.SetDataSource(rows);

                using (Stream reportStream = report.ExportToStream(
     CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                using (MemoryStream memStream = new MemoryStream())
                {
                    reportStream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Streams the generated invoice PDF directly to the browser as a download.
        /// Call this from a page's code-behind (e.g. after Checkout completes).
        /// </summary>
        public static void SendInvoiceToResponse(List<InvoiceReportRow> rows, int orderId, HttpResponse response)
        {
            byte[] pdfBytes = GenerateInvoicePdf(rows);

            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", $"attachment; filename=Invoice_{orderId}.pdf");
            response.BinaryWrite(pdfBytes);
            response.End();
        }
    }
}