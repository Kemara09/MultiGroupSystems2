using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiGroupSystemsTester.Services
{
    public static class SmsService
    {
        private static readonly HttpClient client = new HttpClient();

        public static bool SendSms(string toPhoneNumber, string message)
        {
            try
            {
                return SendSmsAsync(toPhoneNumber, message).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SendSms failed: " + ex.Message);
                throw;
            }
        }

        private static async Task<bool> SendSmsAsync(string toPhoneNumber, string message)
        {
            string apiKey = ConfigurationManager.AppSettings["TextBeeApiKey"];
            var payload = new StringBuilder();
            payload.Append("{");
            payload.Append($"\"recipients\":[\"{Escape(toPhoneNumber)}\"],");
            payload.Append($"\"message\":\"{Escape(message)}\",");
            payload.Append($"\"simSubscriptionId\":4");
            payload.Append("}");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.textbee.dev/api/v1/gateway/send-sms");
            request.Headers.Add("x-api-key", apiKey);
            request.Content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine($"TextBee API error ({(int)response.StatusCode}): {body}");
                throw new Exception($"SMS DEBUG: TextBee API returned {(int)response.StatusCode} - {body}");
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