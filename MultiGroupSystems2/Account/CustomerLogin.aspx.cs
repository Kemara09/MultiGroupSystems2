using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MultiGroupSystemsTester.Account
{
    public partial class CustomerLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = "SELECT CustomerID, FirstName FROM Customer WHERE Username = @Username AND Password = @Password";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Session["CustomerID"] = Convert.ToInt32(reader["CustomerID"]);
                            Session["CustomerName"] = reader["FirstName"].ToString();
                            Response.Redirect("~/Default.aspx");
                        }
                        else
                        {
                            lblError.Text = "Incorrect username or password.";
                            lblError.Visible = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Could not log in right now.";
                lblError.Visible = true;
            }
        }
    }
}