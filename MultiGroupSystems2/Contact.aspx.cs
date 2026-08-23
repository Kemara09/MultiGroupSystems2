using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MultiGroupSystems2
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string connString = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

                string query = "INSERT INTO ContactSubmissions (FirstName, Surname, Email, Message, StaffID) VALUES (@FirstName, @Surname, @Email, @Message, @StaffID)";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(txtFirstName.Text) ? (object)DBNull.Value : txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Surname", string.IsNullOrEmpty(txtSurname.Text) ? (object)DBNull.Value : txtSurname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Message", txtMessage.Text.Trim());
                        cmd.Parameters.AddWithValue("@StaffID", DBNull.Value);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();

                            lblStatus.CssClass = "d-block text-success mt-2 text-center font-weight-bold";
                            lblStatus.Text = "Thank you! Your message has been submitted.";
                            txtFirstName.Text = "";
                            txtSurname.Text = "";
                            txtEmail.Text = "";
                            txtMessage.Text = "";
                        }
                        catch (Exception ex)
                        {
                            lblStatus.CssClass = "d-block text-danger mt-2 text-center font-weight-bold";
                            lblStatus.Text = "An error occurred. Please try again later.";
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }
    }
}