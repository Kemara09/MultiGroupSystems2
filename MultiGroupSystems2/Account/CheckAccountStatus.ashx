<%@ WebHandler Language="C#" Class="MultiGroupSystemsTester.Account.CheckAccountStatus" %>

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace MultiGroupSystemsTester.Account
{
   
    public class CheckAccountStatus : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string email = context.Request.QueryString["email"];
            bool exists = false;
            bool activated = false;

            if (!string.IsNullOrWhiteSpace(email))
            {
                string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
                const string query = "SELECT Password FROM Customer WHERE EmailAddress = @EmailAddress";

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailAddress", email.Trim());
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            exists = true;
                            object pwd = reader["Password"];
                            activated = pwd != DBNull.Value && !string.IsNullOrEmpty(pwd.ToString());
                        }
                    }
                }
            }

            context.Response.Write("{\"exists\":" + exists.ToString().ToLower() +
                                    ",\"activated\":" + activated.ToString().ToLower() + "}");
        }

        public bool IsReusable => false;
    }
}
