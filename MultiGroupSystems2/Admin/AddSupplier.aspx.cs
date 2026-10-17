using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MultiGroupSystemsTester.Admin
{
    public partial class AddSupplier : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

        protected void btnAddSupplier_Click(object sender, EventArgs e)
        {
          
            if (Page.IsValid)
            {

                string connString = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"]?.ConnectionString;


                string query = "INSERT INTO Supplier (SupplierName, ContactPerson, EmailAddress, PhoneNumber, StreetNo, StreetName, Suburb, City, Province, PostalCode) " +
                               "VALUES (@SupplierName, @ContactPerson, @EmailAddress, @PhoneNumber, @StreetNo, @StreetName, @Suburb, @City, @Province, @PostalCode)";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                     
                        cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@EmailAddress", txtEmailAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text.Trim());
                       

                        
                        cmd.Parameters.AddWithValue("@StreetNo", string.IsNullOrEmpty(txtStreetNo.Text) ? (object)DBNull.Value : txtStreetNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@StreetName", txtStreetName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Suburb", string.IsNullOrEmpty(txtSuburb.Text) ? (object)DBNull.Value : txtSuburb.Text.Trim());
                        cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                        cmd.Parameters.AddWithValue("@Province", ddlProvince.SelectedValue);
                        cmd.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(txtNotes.Text) ? (object)DBNull.Value : txtNotes.Text.Trim());

                        try
                        {
                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                lblMessage.Text = "Supplier added successfully!";
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                                ClearForm();
                            }
                            else
                            {
                                lblMessage.Text = "Error adding supplier. Please try again.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "A database error occurred: " + ex.Message;
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
        }

        private void ClearForm()
        {
            txtSupplierName.Text = "";
            txtContactPerson.Text = "";
            txtEmailAddress.Text = "";
            txtPhoneNumber.Text = "";
            ddlSupplierType.SelectedIndex = 0;
            txtStreetNo.Text = "";
            txtStreetName.Text = "";
            txtSuburb.Text = "";
            txtCity.Text = "";
            ddlProvince.SelectedIndex = 0;
            txtPostalCode.Text = "";
            txtNotes.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
          
            Response.Redirect("Dashboard.aspx");
        }
    }
}