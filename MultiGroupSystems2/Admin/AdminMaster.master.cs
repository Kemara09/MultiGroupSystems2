using System;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace MultiGroupSystemsTester.Admin
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminDetails();
            }
            HighlightActiveNavItem(); 
        }

        private void LoadAdminDetails()
        {
            // Full name stored when the administrator logs in
            string fullName = Session["AdminFullName"] as string;


// Role stored when the administrator logs in
string role = Session["AdminRole"] as string;

            // If no session information exists
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = "Staff User";
            }

            if (string.IsNullOrEmpty(role))
            {
                role = "Administrator";
            }

            // Display full name and role
            lblAdminFullName.Text = fullName;
            lblAdminRole.Text = role;

            lblSidebarFullName.Text = fullName;
            lblSidebarRole.Text = role;

            // Create initials from the full name
            lblProfileInitials.Text = GetInitials(fullName);
            lblSidebarInitials.Text = GetInitials(fullName);


}

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "A";


string[] names = fullName.Trim().Split(
    new char[] { ' ' },
    StringSplitOptions.RemoveEmptyEntries
);

            if (names.Length == 1)
            {
                return names[0].Substring(0, 1).ToUpper();
            }

            return (
                names[0].Substring(0, 1) +
                names[names.Length - 1].Substring(0, 1)
            ).ToUpper();


}


        private void HighlightActiveNavItem()
        {
            string currentPage = Path.GetFileName(Request.Path);

            SetActive(navDashboard, "Dashboard.aspx", currentPage);
            SetActive(navProducts, "AddProduct.aspx", currentPage);
            SetActive(navOrders, "OrderLogistics.aspx", currentPage);
            SetActive(navSuppliers, "AddSupplier.aspx", currentPage);
            SetActive(navStaff, "StaffDataCapture.aspx", currentPage);
            SetActive(navReports, "Reports.aspx", currentPage);
        }

        private void SetActive(HtmlAnchor anchor, string pageName, string currentPage)
        {
            if (string.Equals(pageName, currentPage, StringComparison.OrdinalIgnoreCase))
            {
                anchor.Attributes["class"] = "nav-item active";
            }
        }

        protected void btnAdminLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}