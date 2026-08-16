using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MultiGroupSystemsTester.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    const string checkUsername = "SELECT COUNT(*) FROM Customer WHERE Username = @Username";
                    using (SqlCommand cmdCheck = new SqlCommand(checkUsername, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                        {
                            lblError.Text = "That username is already taken.";
                            lblError.Visible = true;
                            return;
                        }
                    }

                    const string insert = @"
    INSERT INTO [dbo].[Customer]
        (FirstName, LastName, PhoneNumber, EmailAddress, StreetNumber, StreetName, Suburb, City, Province, PostalCode, Username, Password)
    VALUES
        (@FirstName, @LastName, @PhoneNumber, @EmailAddress, @StreetNumber, @StreetName, @Suburb, @City, @Province, @PostalCode, @Username, @Password);
    SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@StreetNumber", txtStreetNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@StreetName", txtStreetName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Suburb", txtSuburb.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        cmd.Parameters.AddWithValue("@Province", ddlProvince.SelectedValue);
                        cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                        int newCustomerId = Convert.ToInt32(cmd.ExecuteScalar());

                        Session["CustomerID"] = newCustomerId;
                        Session["CustomerName"] = txtFirstName.Text.Trim();
                        Response.Redirect("~/Default.aspx");
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Could not create your account right now.";
                lblError.Visible = true;
            }
        }
    }
}