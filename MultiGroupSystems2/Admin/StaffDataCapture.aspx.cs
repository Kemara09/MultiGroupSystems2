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

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string connStr = ConfigurationManager.ConnectionStrings["GroupWst14"].ConnectionString;

                string query = "INSERT INTO Staff (FirstName, LastName, Role, EmailAddress, PhoneNumber, Username, Password) " +
                               "VALUES (@FirstName, @LastName, @Role, @EmailAddress, @PhoneNumber, @Username, @Password)";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Role", ddlRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();

                            Response.Redirect("Dashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        catch (Exception ex)
                        {
                            Response.Write("<script>alert('Error saving staff member: " + ex.Message.Replace("'", "\\'") + "');</script>");
                        }
                    }
                }
            }
        }
    }
}