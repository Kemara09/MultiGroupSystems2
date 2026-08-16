using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;

namespace MultiGroupSystemsTester.Account
{
    public partial class CustomerLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie rememberedUser = Request.Cookies["RememberedUsername"];
                if (rememberedUser != null && !string.IsNullOrEmpty(rememberedUser.Value))
                {
                    txtUsername.Text = rememberedUser.Value;
                    chkRememberMe.Checked = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string recaptchaToken = Request.Form["g-recaptcha-response"];
            if (string.IsNullOrEmpty(recaptchaToken) || !VerifyRecaptcha(recaptchaToken))
            {
                lblError.Text = "Please complete the CAPTCHA verification.";
                lblError.Visible = true;
                return;
            }

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

                            if (chkRememberMe.Checked)
                            {
                                HttpCookie rememberCookie = new HttpCookie("RememberedUsername", txtUsername.Text.Trim());
                                rememberCookie.Expires = DateTime.Now.AddDays(30);
                                Response.Cookies.Add(rememberCookie);
                            }
                            else
                            {
                                HttpCookie clearCookie = new HttpCookie("RememberedUsername", "");
                                clearCookie.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(clearCookie);
                            }

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

        private bool VerifyRecaptcha(string token)
        {
            try
            {
                string secretKey = ConfigurationManager.AppSettings["RecaptchaSecretKey"];

                using (var client = new WebClient())
                {
                    var values = new System.Collections.Specialized.NameValueCollection
                    {
                        { "secret", secretKey },
                        { "response", token }
                    };

                    byte[] result = client.UploadValues("https://www.google.com/recaptcha/api/siteverify", values);
                    string jsonResponse = System.Text.Encoding.UTF8.GetString(result);

                    JObject parsed = JObject.Parse(jsonResponse);
                    return (bool)parsed["success"];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("reCAPTCHA verify failed: " + ex.Message);
                return false;
            }
        }
    }
}