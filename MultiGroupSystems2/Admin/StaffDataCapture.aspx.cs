using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace MultiGroupSystemsTester.Admin
{
    public partial class StaffDataCapture : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string connStr = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;
            string username = txtUsername.Text.Trim();

            // The password is left empty on purpose: an empty password means "not activated yet".
            // The staff member sets their own (hashed with PasswordHasher) on StaffActivate.aspx
            // after verifying the SMS code.
            string query = "INSERT INTO Staff (FirstName, LastName, Role, EmailAddress, PhoneNumber, Username, Password) " +
                           "VALUES (@FirstName, @LastName, @Role, @EmailAddress, @PhoneNumber, @Username, @Password)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Activation finds the account by username, so two staff can't share one
                    using (SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Staff WHERE Username = @Username", conn))
                    {
                        check.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;

                        if ((int)check.ExecuteScalar() > 0)
                        {
                            ShowAlert("That username is already taken. Please choose a different one.");
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Role", ddlRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", string.Empty);

                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("Dashboard.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                ShowAlert("Error saving staff member: " + ex.Message);
            }
        }

        private void ShowAlert(string message)
        {
            Response.Write("<script>alert('" + HttpUtility.JavaScriptStringEncode(message) + "');</script>");
        }
    }
}