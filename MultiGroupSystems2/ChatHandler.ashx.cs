using MultiGroupSystemsTester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace MultiGroupSystemsTester
{
    public class ChatHandler : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!string.Equals(context.Request.HttpMethod, "POST",
                    StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 405;
                return;
            }

            ChatReply result;

            try
            {
                string message = context.Request.Form["message"];

                // The customer ID ALWAYS comes from the server-side session,
                // never from anything the browser sends.
                int? customerId = null;
                if (context.Session != null && context.Session["CustomerID"] != null)
                {
                    customerId = Convert.ToInt32(context.Session["CustomerID"]);
                }

                List<CartItem> cart = context.Session?["Cart"] as List<CartItem>;

                result = ChatbotService.GetReply(message, customerId, cart);
            }
            catch (Exception)
            {
                result = new ChatReply
                {
                    Reply = "Sorry, something went wrong on my side. Please try again in a moment."
                };
            }

            var payload = new
            {
                reply = result.Reply,
                links = result.Links.Select(l => new { text = l.Text, url = l.Url })
            };

            context.Response.Write(new JavaScriptSerializer().Serialize(payload));
        }
    }
}