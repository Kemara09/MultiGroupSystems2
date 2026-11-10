using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MultiGroupSystemsTester.Admin
{
    public partial class AddProduct : System.Web.UI.Page
    {
        
        private string connString = ConfigurationManager.ConnectionStrings["GroupWst14ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSuppliers();
            }
        }

        private void BindSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT SupplierID, SupplierName FROM Supplier ORDER BY SupplierName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlSupplier.DataSource = dt;
                        ddlSupplier.DataTextField = "SupplierName";
                        ddlSupplier.DataValueField = "SupplierID";
                        ddlSupplier.DataBind();

                       
                        ddlSupplier.Items.Insert(0, new ListItem("Select supplier", ""));
                    }
                }
            }
        }

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            
            if (!Page.IsValid)
            {
                return;
            }

            string productName = txtProductName.Text.Trim();
            string category = ddlCategory.SelectedValue;
            decimal price = decimal.Parse(txtPrice.Text.Trim());
            string description = txtDescription.Text.Trim();
            int stockQuantity = int.Parse(txtStock.Text.Trim());
            int reorderLevel = int.Parse(txtReorderLevel.Text.Trim());
            int supplierID = int.Parse(ddlSupplier.SelectedValue);


            string imagePath = string.Empty;

            
            if (fileProductImage.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fileProductImage.PostedFile.FileName);
                    string extension = Path.GetExtension(fileName).ToLower();

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        string saveFolder = Server.MapPath("~/Uploads/Products/");
                        if (!Directory.Exists(saveFolder))
                        {
                            Directory.CreateDirectory(saveFolder);
                        }

                        string uniqueFileName = DateTime.Now.Ticks.ToString() + "_" + fileName;
                        string fullPath = Path.Combine(saveFolder, uniqueFileName);

                        fileProductImage.SaveAs(fullPath);
                        imagePath = "~/Uploads/Products/" + uniqueFileName;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }

            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"INSERT INTO Inventory 
                                 (ProductName, Category, UnitPrice, Description, QuantityInStock, ReorderLevel, SupplierID, ImagePath) 
                                 VALUES 
                                 (@ProductName, @Category, @UnitPrice, @Description, @QuantityInStock, @ReorderLevel, @SupplierID, @ImagePath)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(category) ? (object)DBNull.Value : category);
                    cmd.Parameters.AddWithValue("@UnitPrice", price);
                    cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);
                    cmd.Parameters.AddWithValue("@QuantityInStock", stockQuantity);
                    cmd.Parameters.AddWithValue("@ReorderLevel", reorderLevel);
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    cmd.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(imagePath) ? (object)DBNull.Value : imagePath);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            
            Response.Redirect("Dashboard.aspx");
        }
    }
}