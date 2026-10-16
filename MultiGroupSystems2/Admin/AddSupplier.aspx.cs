using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace MultiGroupSystemsTester.Admin
{
    public partial class AddSupplier : AdminPage
    {
        string connString = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"]?.ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnAddSupplier_Click(object sender, EventArgs e)
        {
            
             
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

       
    }
}