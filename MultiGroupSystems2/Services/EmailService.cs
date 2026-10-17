using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MultiGroupSystemsTester.Services
{
    public static class EmailService
    {
        static EmailService()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public static bool SendInvoiceEmail(string toEmail, string toName, int orderId, byte[] invoicePdf)
        {
            try
            {
                string orderIdStr = orderId.ToString();
                string subject = "Your Invoice - Order #" + orderIdStr;
                string body =
                    "<p>Hi " + WebUtility.HtmlEncode(toName ?? "") + ",</p>" +
                    "<p>Thank you for your order. Please find your invoice for order #" +
                    WebUtility.HtmlEncode(orderIdStr) + " attached.</p>" +
                    "<p>Multigroup Inc.</p>";

                using (var pdfStream = new MemoryStream(invoicePdf))
                using (var attachment = new Attachment(pdfStream, "Invoice-" + orderIdStr + ".pdf", "application/pdf"))
                {
                    Send(toEmail, toName, subject, body, attachment);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SendInvoiceEmail failed: " + ex.Message);
                throw;
            }
        }

        public static bool SendPasswordResetEmail(string toEmail, string toName, string resetLink)
        {
            try
            {
                string subject = "Reset your Multigroup Inc. password";
                string body =
                    "<p>Hi " + WebUtility.HtmlEncode(toName ?? "") + ",</p>" +
                    "<p>We received a request to reset the password on your Multigroup Inc. account. " +
                    "Click the button below to choose a new one. The link expires in 30 minutes and can only be used once.</p>" +
                    "<p><a href=\"" + resetLink + "\" style=\"background:#0F6E56;color:#ffffff;padding:10px 18px;" +
                    "border-radius:6px;text-decoration:none;display:inline-block;\">Reset password</a></p>" +
                    "<p>If you didn't ask for this, you can ignore this email. Your password won't change.</p>" +
                    "<p>Multigroup Inc.</p>";

                Send(toEmail, toName, subject, body, null);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SendPasswordResetEmail failed: " + ex.Message);
                throw;
            }
        }

        private static void Send(string toEmail, string toName, string subject, string htmlBody, Attachment attachment)
        {
            string gmailAddress = ConfigurationManager.AppSettings["GmailAddress"];
            string appPassword = ConfigurationManager.AppSettings["GmailAppPassword"];
            string fromName = ConfigurationManager.AppSettings["SmtpFromName"] ?? "Multigroup Inc.";

            if (string.IsNullOrWhiteSpace(gmailAddress) || string.IsNullOrWhiteSpace(appPassword))
                throw new InvalidOperationException("EMAIL CONFIG: GmailAddress or GmailAppPassword is missing from app settings.");

            appPassword = appPassword.Replace(" ", "");

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(gmailAddress, fromName);
                message.To.Add(string.IsNullOrWhiteSpace(toName)
                    ? new MailAddress(toEmail)
                    : new MailAddress(toEmail, toName));
                message.Subject = subject;
                message.Body = htmlBody;
                message.IsBodyHtml = true;

                if (attachment != null)
                    message.Attachments.Add(attachment);

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(gmailAddress, appPassword);
                    smtp.Timeout = 20000;
                    smtp.Send(message);
                }
            }
        }
    }
}