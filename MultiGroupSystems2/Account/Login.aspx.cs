using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace MultiGroupSystemsTester.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) 
        {
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
const string query = "SELECT StaffID, FirstName, LastName, Role FROM Staff WHERE Username = @Username AND Password = @Password";
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
                                string fullName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                                string role = reader["Role"].ToString();
                                reader.Close();

                                Session["AdminFullName"] = fullName;
                                Session["AdminRole"] = role;

                                
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1,                                  
                                txtUsername.Text.Trim(),           
                                DateTime.Now,                     
                                DateTime.Now.AddMinutes(30),       
                                false,                              
                                role,                               
                                FormsAuthentication.FormsCookiePath
                            );

                            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            Response.Cookies.Add(authCookie);

                            string returnUrl = Request.QueryString["ReturnUrl"];
                            Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/Admin/Dashboard.aspx");
                        }
                        else
                        {
                            lblError.Text = "Incorrect username or password.";
                            lblError.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                lblError.Text = "Could not log in right now: ";
                lblError.Visible = true;
            }
        }
    }
}