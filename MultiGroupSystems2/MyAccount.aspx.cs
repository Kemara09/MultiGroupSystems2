using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MultiGroupSystemsTester
{
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustomerID"] == null)
            {
                Response.Redirect("~/Account/CustomerLogin.aspx");
                return;
            }

            if (!IsPostBack) LoadCustomerDetails();
        }

        private void LoadCustomerDetails()
        {
            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string query = "SELECT FirstName, LastName, PhoneNumber, EmailAddress, StreetNumber, StreetName, Suburb, City, Province, PostalCode FROM Customer WHERE CustomerID = @CustomerID";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", Session["CustomerID"]);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtPhone.Text = reader["PhoneNumber"].ToString();
                        txtEmail.Text = reader["EmailAddress"].ToString();
                        txtStreetNumber.Text = reader["StreetNumber"].ToString();
                        txtStreetName.Text = reader["StreetName"].ToString();
                        txtSuburb.Text = reader["Suburb"].ToString();
                        txtCity.Text = reader["City"].ToString();
                        ddlProvince.SelectedValue = reader["Province"].ToString();
                        txtPostalCode.Text = reader["PostalCode"].ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            const string update = @"
                UPDATE Customer SET
                    FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber,
                    EmailAddress = @EmailAddress, StreetNumber = @StreetNumber, StreetName = @StreetName,
                    Suburb = @Suburb, City = @City, Province = @Province, PostalCode = @PostalCode
                WHERE CustomerID = @CustomerID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(update, conn))
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
                    cmd.Parameters.AddWithValue("@CustomerID", Session["CustomerID"]);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Session["CustomerName"] = txtFirstName.Text.Trim();
                lblSuccess.Text = "Your details have been updated.";
                lblSuccess.Visible = true;
                lblError.Visible = false;
            }
            catch (Exception)
            {
                lblError.Text = "Could not save your changes right now.";
                lblError.Visible = true;
                lblSuccess.Visible = false;
            }
        }
    }
}