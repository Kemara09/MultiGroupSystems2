using System;

namespace MultiGroupSystemsTester.Models
{
    public class InvoiceReportRow
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public string VATNumber { get; set; }
        public int InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string StaffName { get; set; }
        public string SubtotalStr { get; set; }
        public string VATAmountStr { get; set; }
        public string GrandTotalStr { get; set; }
        public int LineNumber { get; set; }
        public string ProductName { get; set; }
        public string UnitPriceStr { get; set; }
        public int Quantity { get; set; }
        public string LineSubtotalStr { get; set; }
        public string LineVATStr { get; set; }
        public string LineTotalStr { get; set; }
    }
}