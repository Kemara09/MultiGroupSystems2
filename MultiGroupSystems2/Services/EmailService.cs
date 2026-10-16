using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MultiGroupSystemsTester.Services
{
    public static class EmailService
    {
        private static readonly HttpClient client = new HttpClient();

        public static bool SendInvoiceEmail(string toEmail, string toName, int orderId, byte[] invoicePdf)
        {
            try
            {
                return SendInvoiceEmailAsync(toEmail, toName, orderId, invoicePdf).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SendInvoiceEmail failed: " + ex.Message);
                throw;
            }
        }

        private static async Task<bool> SendInvoiceEmailAsync(string toEmail, string toName, int orderId, byte[] invoicePdf)
        {
            string apiKey = ConfigurationManager.AppSettings["BrevoApiKey"];
            string senderEmail = ConfigurationManager.AppSettings["SmtpFromEmail"];
            string senderName = ConfigurationManager.AppSettings["SmtpFromName"] ?? "Multigroup Inc.";

            string orderIdStr = orderId.ToString();
            string subject = $"Your Invoice - Order #{orderIdStr}";
            string htmlContent = $"<p>Hi {Escape(toName)},</p><p>Thank you for your order. Please find your invoice for order #{Escape(orderIdStr)} attached.</p><p>Multigroup Inc.</p>";
            string pdfBase64 = Convert.ToBase64String(invoicePdf);

            var payload = new StringBuilder();
            payload.Append("{");
            payload.Append($"\"sender\":{{\"name\":\"{Escape(senderName)}\",\"email\":\"{Escape(senderEmail)}\"}},");
            payload.Append($"\"to\":[{{\"email\":\"{Escape(toEmail)}\",\"name\":\"{Escape(toName)}\"}}],");
            payload.Append($"\"subject\":\"{Escape(subject)}\",");
            payload.Append($"\"htmlContent\":\"{Escape(htmlContent)}\",");
            payload.Append($"\"attachment\":[{{\"content\":\"{pdfBase64}\",\"name\":\"Invoice-{Escape(orderIdStr)}.pdf\"}}]");
            payload.Append("}");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", apiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine($"Brevo API error ({(int)response.StatusCode}): {body}");
                throw new Exception($"EMAIL DEBUG: Brevo API returned {(int)response.StatusCode} - {body}");
            }

            return true;
        }

        public static bool SendPasswordResetEmail(string toEmail, string toName, string resetLink)
        {
            try
            {
                return SendPasswordResetEmailAsync(toEmail, toName, resetLink).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SendPasswordResetEmail failed: " + ex.Message);
                throw;
            }
        }

        private static async Task<bool> SendPasswordResetEmailAsync(string toEmail, string toName, string resetLink)
        {
            string apiKey = ConfigurationManager.AppSettings["BrevoApiKey"];
            string senderEmail = ConfigurationManager.AppSettings["SmtpFromEmail"];
            string senderName = ConfigurationManager.AppSettings["SmtpFromName"] ?? "Multigroup Inc.";

            string subject = "Reset your Multigroup Inc. password";
            string htmlContent =
                "<p>Hi " + System.Net.WebUtility.HtmlEncode(toName) + ",</p>" +
                "<p>We received a request to reset the password on your Multigroup Inc. account. " +
                "Click the button below to choose a new one. The link expires in 30 minutes and can only be used once.</p>" +
                "<p><a href=\"" + resetLink + "\" style=\"background:#0F6E56;color:#ffffff;padding:10px 18px;" +
                "border-radius:6px;text-decoration:none;display:inline-block;\">Reset password</a></p>" +
                "<p>If you didn't ask for this, you can ignore this email. Your password won't change.</p>" +
                "<p>Multigroup Inc.</p>";

            var payload = new StringBuilder();
            payload.Append("{");
            payload.Append($"\"sender\":{{\"name\":\"{Escape(senderName)}\",\"email\":\"{Escape(senderEmail)}\"}},");
            if (string.IsNullOrWhiteSpace(toName))
                payload.Append($"\"to\":[{{\"email\":\"{Escape(toEmail)}\"}}],");
            else
                payload.Append($"\"to\":[{{\"email\":\"{Escape(toEmail)}\",\"name\":\"{Escape(toName)}\"}}],");
            payload.Append($"\"subject\":\"{Escape(subject)}\",");
            payload.Append($"\"htmlContent\":\"{Escape(htmlContent)}\"");
            payload.Append("}");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
            request.Headers.Add("api-key", apiKey);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine($"Brevo API error ({(int)response.StatusCode}): {body}");
                throw new Exception($"EMAIL DEBUG: Brevo API returned {(int)response.StatusCode} - {body}");
            }

            return true;
        }

        private static string Escape(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "");
        }
    }
}