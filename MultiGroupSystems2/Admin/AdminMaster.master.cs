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

            ApplyRoleAccess();
            HighlightActiveNavItem();
        }

        private void LoadAdminDetails()
        {
            
            string fullName = Session["AdminFullName"] as string;

            
            string role = Session["AdminRole"] as string;

            
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = "Staff User";
            }

            if (string.IsNullOrEmpty(role))
            {
                role = "Administrator";
            }

            
            lblAdminFullName.Text = fullName;
            lblAdminRole.Text = role;

            lblSidebarFullName.Text = fullName;
            lblSidebarRole.Text = role;

            
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

        
        private void ApplyRoleAccess()
        {
            string role = Session["AdminRole"] as string;

            navDashboard.Visible = AdminAccess.CanAccess(role, "Dashboard");
            navProducts.Visible = AdminAccess.CanAccess(role, "AddProduct");
            navOrders.Visible = AdminAccess.CanAccess(role, "OrderLogistics");
            navViewOrders.Visible = AdminAccess.CanAccess(role, "ViewOrders");
            navSuppliers.Visible = AdminAccess.CanAccess(role, "AddSupplier");
            navStaff.Visible = AdminAccess.CanAccess(role, "StaffDataCapture");
            navReports.Visible = AdminAccess.CanAccess(role, "Reports");
        }

        private void HighlightActiveNavItem()
        {
            
            string currentPage = Path.GetFileNameWithoutExtension(Request.Path);

            SetActive(navDashboard, "Dashboard", currentPage);
            SetActive(navProducts, "AddProduct", currentPage);
            SetActive(navOrders, "OrderLogistics", currentPage);
            SetActive(navViewOrders, "ViewOrders", currentPage);
            SetActive(navSuppliers, "AddSupplier", currentPage);
            SetActive(navStaff, "StaffDataCapture", currentPage);
            SetActive(navReports, "Reports", currentPage);
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

            
            Session.Clear();
            Session.Abandon();

            Response.Redirect("~/Default.aspx");
        }
    }
}   