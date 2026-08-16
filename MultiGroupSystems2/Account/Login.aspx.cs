using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace MultiGroupSystemsTester.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = "SELECT StaffID FROM Staff WHERE Username = @Username AND Password = @Password";
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
                            FormsAuthentication.RedirectFromLoginPage(txtUsername.Text.Trim(), false);
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