using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;

namespace MultiGroupSystemsTester
{
    public class StaffStatus : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 405;
                return;
            }

            bool needsActivation = false;

            try
            {
                // Simple throttle: at most 30 checks a minute per IP address
                string ip = context.Request.UserHostAddress ?? "unknown";
                string key = "staffstatus:" + ip;
                int count = (HttpRuntime.Cache[key] as int?) ?? 0;

                if (count < 30)
                {
                    HttpRuntime.Cache.Insert(key, count + 1, null,
                        DateTime.UtcNow.AddMinutes(1), Cache.NoSlidingExpiration);

                    string username = (context.Request.Form["username"] ?? "").Trim();

                    if (username.Length > 0 && username.Length <= 50)
                    {
                        string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

                        using (var conn = new SqlConnection(connStr))
                        using (var cmd = new SqlCommand("SELECT Password FROM Staff WHERE Username = @Username", conn))
                        {
                            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                            conn.Open();

                            using (var reader = cmd.ExecuteReader())
                            {
                                // Only an existing account with an empty password "needs activation".
                                // A missing username gets the same answer as an activated one (false).
                                if (reader.Read())
                                {
                                    object pw = reader["Password"];
                                    needsActivation = pw == DBNull.Value || string.IsNullOrEmpty(pw.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("StaffStatus error: " + ex.Message);
            }

            context.Response.Write(new JavaScriptSerializer().Serialize(new { needsActivation = needsActivation }));
        }
    }
}