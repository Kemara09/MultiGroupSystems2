using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MultiGroupSystemsTester.Account
{
    public partial class Activate : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlFindAccount.Visible = true;
                pnlSetPassword.Visible = false;

                string prefillEmail = Request.QueryString["email"];
                if (!string.IsNullOrEmpty(prefillEmail))
                {
                    txtEmail.Text = prefillEmail;
                }
            }
        }

        protected void btnFindAccount_Click(object sender, EventArgs e)
        {
            lblFindError.Text = "";
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblFindError.Text = "Please enter your email address.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                const string query = "SELECT CustomerID, Password FROM Customer WHERE EmailAddress = @EmailAddress";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmailAddress", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            lblFindError.Text = "We couldn't find an account with that email address.";
                            return;
                        }

                        int customerId = reader.GetInt32(reader.GetOrdinal("CustomerID"));
                        object passwordObj = reader["Password"];
                        bool alreadyActivated = passwordObj != DBNull.Value && !string.IsNullOrEmpty(passwordObj.ToString());

                        if (alreadyActivated)
                        {
                            lblFindError.Text = "This account has already been activated. Please log in instead.";
                            return;
                        }

                        ViewState["ActivateCustomerId"] = customerId;
                        hdnEmail.Value = email;
                    }
                }
            }

            pnlFindAccount.Visible = false;
            pnlSetPassword.Visible = true;
        }

        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            lblPasswordError.Text = "";

            if (ViewState["ActivateCustomerId"] == null)
            {
                lblPasswordError.Text = "Your session expired. Please start again.";
                pnlFindAccount.Visible = true;
                pnlSetPassword.Visible = false;
                return;
            }

            int customerId = (int)ViewState["ActivateCustomerId"];
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                lblPasswordError.Text = "Password must be at least 8 characters long.";
                return;
            }

            if (password != confirmPassword)
            {
                lblPasswordError.Text = "Passwords do not match.";
                return;
            }

            string hashedPassword = PasswordHasher.HashPassword(password);

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                // The Password IS NULL / '' guard prevents a race where the account
                // gets activated twice between step 1 and step 2.
                const string query = @"UPDATE Customer 
                                        SET Password = @Password 
                                        WHERE CustomerID = @CustomerID 
                                        AND (Password IS NULL OR Password = '')";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        lblPasswordError.Text = "This account has already been activated. Please log in instead.";
                        return;
                    }
                }
            }

            Response.Redirect("~/Account/CustomerLogin.aspx?activated=1");
        }
    }
}