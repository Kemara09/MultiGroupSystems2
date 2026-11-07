using MultiGroupSystemsTester.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MultiGroupSystemsTester
{
    public class ChatLink
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }

    public class ChatReply
    {
        public string Reply { get; set; }
        public List<ChatLink> Links { get; set; } = new List<ChatLink>();
    }

    public static class ChatbotService
    {
        private static readonly CultureInfo ZA = CultureInfo.GetCultureInfo("en-ZA");

        private static string ConnStr =>
            ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

        private static readonly string[] StopWords =
        {
            "a","an","the","do","you","have","has","any","got","sell","stock","in","is","are",
            "i","me","my","we","for","of","to","on","at","it","and","or","please","can","could",
            "what","whats","how","much","does","cost","price","prices","looking","want","need",
            "find","show","search","there","some","about","with","buy","available","availability"
        };

        // ===================================================================
        // Main entry point
        // ===================================================================
        public static ChatReply GetReply(string message, int? customerId, List<CartItem> cart)
        {
            string text = (message ?? "").Trim();

            if (text.Length == 0)
                return Say("Type a question and I'll do my best to help.");

            if (text.Length > 300)
                text = text.Substring(0, 300);

            string lower = text.ToLowerInvariant();
            bool loggedIn = customerId.HasValue;

            // 1. Greetings (short messages only, so "hi, where is my order" isn't swallowed)
            if (WordCount(lower) <= 3 && Has(lower, "hi", "hello", "hey", "howzit", "good morning", "good afternoon", "good evening"))
                return Greeting(customerId);

            if (Has(lower, "thanks", "thank you", "cheers", "appreciate"))
                return Say("You're welcome! Is there anything else I can help with?");

            // 2. Personalised intents
            if (Has(lower, "my order", "my orders", "order status", "track", "tracking", "where is my", "my purchases", "recent order", "last order")
                || Regex.IsMatch(lower, @"\border\s*#?\s*\d+"))
            {
                return OrdersReply(customerId, lower);
            }

            if (Has(lower, "wishlist", "wish list", "saved items", "favourites", "favorites"))
                return WishlistReply(customerId);

            if (Has(lower, "my cart", "my basket", "shopping cart", "in my cart", "cart total"))
                return CartReply(cart);

            if (Has(lower, "my account", "my details", "my profile", "my email", "log in", "login", "sign in", "register", "sign up"))
                return AccountReply(customerId);

            // 3. General FAQs (edit these to match your real policies)
            if (Has(lower, "delivery", "deliver", "shipping", "collect", "collection"))
            {
                return Say(
                    "We offer Delivery or Collection at checkout. You choose your delivery date when you check out, " +
                    "and you can add delivery instructions for the driver.\n\n" +
                    (loggedIn
                        ? "You can also ask me \"where is my order?\" for the latest status."
                        : "Log in and I can also check the status of your orders."));
            }

            if (Has(lower, "return", "refund", "exchange", "faulty", "warranty"))
            {
                return Say(
                    "If something is faulty or not what you ordered, please contact us as soon as possible with your " +
                    "order number so we can arrange a return, exchange or refund.");
            }

            if (Has(lower, "pay", "payment", "eft", "card", "checkout", "check out"))
            {
                return Say(
                    "You can browse and add items to your cart without an account, but you'll need to log in or register " +
                    "to complete checkout.",
                    new ChatLink { Text = "View my cart", Url = Url("~/Cart.aspx") });
            }

            if (Has(lower, "contact", "phone", "call", "email us", "hours", "open", "location", "address", "support"))
            {
                return Say(
                    "You can reach the MultiGroup team through the contact details on our site. " +
                    "I can also help with products, your cart and your orders.");
            }

            if (Has(lower, "help", "what can you do", "options", "menu"))
                return HelpReply(loggedIn);

            // 4. Product questions, and the fallback for anything unrecognised
            return ProductReply(text);
        }

        // ===================================================================
        // Intent handlers
        // ===================================================================
        private static ChatReply Greeting(int? customerId)
        {
            string name = customerId.HasValue ? GetFirstName(customerId.Value) : null;

            if (!string.IsNullOrEmpty(name))
            {
                return Say(
                    $"Hi {name}! I'm the MultiGroup assistant. I can check your orders, wishlist and cart, " +
                    "or help you find a product. What can I do for you?");
            }

            return Say(
                "Hi! I'm the MultiGroup assistant. I can help you find products, check your cart and answer " +
                "questions about delivery. Log in and I can also look up your orders. What can I do for you?");
        }

        private static ChatReply HelpReply(bool loggedIn)
        {
            return Say(
                "Here's what I can help with:\n" +
                "• Finding products, prices and stock (e.g. \"do you have CCTV cameras?\")\n" +
                "• What's in your cart\n" +
                "• Delivery and returns information\n" +
                (loggedIn
                    ? "• Your orders and wishlist (e.g. \"where is my order?\")"
                    : "• Your orders and wishlist (log in to use these)"));
        }

        private static ChatReply AccountReply(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NeedLogin("You're not logged in at the moment. Log in or register to see your details, " +
                                 "orders and wishlist, and to check out.");
            }

            string name = GetFirstName(customerId.Value);
            return Say($"You're logged in{(string.IsNullOrEmpty(name) ? "" : " as " + name)}. " +
                       "Ask me about your orders or wishlist any time.");
        }

        private static ChatReply OrdersReply(int? customerId, string lower)
        {
            if (!customerId.HasValue)
            {
                return NeedLogin("I can only look up orders for logged-in customers, so your details stay private. " +
                                 "Please log in and ask me again.");
            }

            // If they mention a specific order number, look up only that one,
            // and ONLY if it belongs to this customer.
            int? orderId = null;
            Match m = Regex.Match(lower, @"\b(\d{1,9})\b");
            if (m.Success && lower.Contains("order"))
                orderId = int.Parse(m.Groups[1].Value);

            string sql =
                "SELECT TOP 3 OrderID, OrderDate, OrderStatus, DeliveryStatus, TotalAmount, DeliveryDate " +
                "FROM [Order] WHERE CustomerID = @CustomerID " +
                (orderId.HasValue ? "AND OrderID = @OrderID " : "") +
                "ORDER BY OrderDate DESC";

            var lines = new List<string>();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId.Value;
                if (orderId.HasValue)
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId.Value;

                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string date = r["OrderDate"] == DBNull.Value
                            ? "date not recorded"
                            : Convert.ToDateTime(r["OrderDate"]).ToString("dd MMM yyyy", ZA);

                        string orderStatus = r["OrderStatus"] == DBNull.Value
                            ? "status unavailable"
                            : r["OrderStatus"].ToString();

                        string line = $"Order #{r["OrderID"]} ({date}): {orderStatus}";

                        if (r["DeliveryStatus"] != DBNull.Value)
                            line += $", delivery: {r["DeliveryStatus"]}";

                        if (r["TotalAmount"] != DBNull.Value)
                            line += $", {string.Format(ZA, "R {0:N2}", r["TotalAmount"])}";

                        if (r["DeliveryDate"] != DBNull.Value)
                            line += $", expected {Convert.ToDateTime(r["DeliveryDate"]).ToString("dd MMM yyyy", ZA)}";

                        lines.Add(line);
                    }
                }
            }

            if (lines.Count == 0)
            {
                return Say(orderId.HasValue
                    ? $"I couldn't find order #{orderId} on your account. Please double-check the order number."
                    : "I don't see any orders on your account yet. Browse our catalogue and place your first one!",
                    new ChatLink { Text = "Browse products", Url = Url("~/Products.aspx") });
            }

            string header = orderId.HasValue ? "Here's that order:\n" : "Here are your most recent orders:\n";
            return Say(header + string.Join("\n", lines));
        }

        private static ChatReply WishlistReply(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NeedLogin("Wishlists are saved to your account. Please log in to see yours.");
            }

            var items = new List<string>();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT TOP 5 i.ProductName, i.UnitPrice
                  FROM Wishlist w
                  INNER JOIN Inventory i ON i.ProductID = w.ProductID
                  WHERE w.CustomerID = @CustomerID
                  ORDER BY w.DateAdded DESC", conn))
            {
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId.Value;
                conn.Open();

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                        items.Add($"• {r["ProductName"]} ({string.Format(ZA, "R {0:N2}", r["UnitPrice"])})");
                }
            }

            if (items.Count == 0)
                return Say("Your wishlist is empty. Tap \"Add to Wishlist\" on any product to save it for later.");

            return Say("Here's what's on your wishlist:\n" + string.Join("\n", items));
        }

        private static ChatReply CartReply(List<CartItem> cart)
        {
            if (cart == null || cart.Count == 0)
            {
                return Say("Your cart is empty right now.",
                    new ChatLink { Text = "Browse products", Url = Url("~/Products.aspx") });
            }

            decimal total = cart.Sum(c => c.UnitPrice * c.Quantity);
            var lines = cart.Select(c =>
                $"• {c.Quantity} x {c.ProductName} ({string.Format(ZA, "R {0:N2}", c.UnitPrice * c.Quantity)})");

            return Say(
                "Here's your cart:\n" + string.Join("\n", lines) +
                $"\nTotal: {string.Format(ZA, "R {0:N2}", total)}",
                new ChatLink { Text = "Go to my cart", Url = Url("~/Cart.aspx") });
        }

        private static ChatReply ProductReply(string text)
        {
            var words = Regex.Matches(text.ToLowerInvariant(), @"[a-z0-9]+")
                             .Cast<Match>()
                             .Select(m => m.Value)
                             .Where(w => w.Length > 1 && !StopWords.Contains(w))
                             .Take(4)
                             .ToList();

            if (words.Count == 0)
                return HelpReply(false);

            // Every keyword must match the product name or category
            var conditions = new List<string>();
            for (int i = 0; i < words.Count; i++)
                conditions.Add($"(ProductName LIKE @w{i} OR Category LIKE @w{i})");

            string sql =
                "SELECT TOP 5 ProductName, UnitPrice, QuantityInStock FROM Inventory WHERE " +
                string.Join(" AND ", conditions) + " ORDER BY ProductName";

            var lines = new List<string>();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                for (int i = 0; i < words.Count; i++)
                {
                    string escaped = words[i].Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
                    cmd.Parameters.Add("@w" + i, SqlDbType.NVarChar, 100).Value = "%" + escaped + "%";
                }

                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        bool inStock = Convert.ToInt32(r["QuantityInStock"]) > 0;
                        lines.Add($"• {r["ProductName"]}: {string.Format(ZA, "R {0:N2}", r["UnitPrice"])} " +
                                  (inStock ? "(in stock)" : "(out of stock)"));
                    }
                }
            }

            if (lines.Count == 0)
            {
                return Say(
                    "Sorry, I couldn't find anything matching that. Try a different name or a category, " +
                    "or ask me \"help\" to see what I can do.",
                    new ChatLink { Text = "Browse all products", Url = Url("~/Products.aspx") });
            }

            return Say("Here's what I found:\n" + string.Join("\n", lines),
                new ChatLink { Text = "See in catalogue", Url = Url("~/Products.aspx") });
        }

        // ===================================================================
        // Helpers
        // ===================================================================
        private static string GetFirstName(int customerId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT FirstName FROM Customer WHERE CustomerID = @CustomerID", conn))
                {
                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result == null || result == DBNull.Value ? null : result.ToString();
                }
            }
            catch
            {
                // If the lookup fails, the bot still works, just without the name.
                return null;
            }
        }

        private static bool Has(string text, params string[] phrases)
        {
            foreach (string p in phrases)
            {
                if (Regex.IsMatch(text, @"\b" + Regex.Escape(p) + @"\b"))
                    return true;
            }
            return false;
        }

        private static int WordCount(string text) =>
            text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

        private static string Url(string virtualPath) =>
            VirtualPathUtility.ToAbsolute(virtualPath);

        private static ChatReply Say(string reply, params ChatLink[] links)
        {
            var r = new ChatReply { Reply = reply };
            r.Links.AddRange(links);
            return r;
        }

        private static ChatReply NeedLogin(string reply) =>
            Say(reply, new ChatLink { Text = "Log in", Url = Url("~/Account/CustomerLogin.aspx") });
    }
}