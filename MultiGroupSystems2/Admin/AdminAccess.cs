using System;
using System.Collections.Generic;

namespace MultiGroupSystemsTester.Admin
{
    /// <summary>
    /// One place that decides which role can open which admin page.
    /// Page names are the .aspx file name without the extension.
    /// </summary>
    public static class AdminAccess
    {
        // These roles can open every admin page.
        private static readonly string[] FullAccessRoles =
        {
            "Manager",
            "Owner",
            "Tester"
        };

        // Everyone else only gets the pages listed for their role.
        // A role that is not listed here (or is null) gets no pages.
        private static readonly Dictionary<string, string[]> RolePages =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "System Administrator", new[] { "Dashboard", "AddProduct", "OrderLogistics", "ViewOrders", "AddSupplier", "Reports" } }
            
        };

        public static bool CanAccess(string role, string page)
        {
            // Everyone who is logged in may see the "access denied" page itself
            if (string.Equals(page, "AccessDenied", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.IsNullOrWhiteSpace(role))
                return false;

            role = role.Trim();

            foreach (string full in FullAccessRoles)
                if (string.Equals(role, full, StringComparison.OrdinalIgnoreCase))
                    return true;

            string[] pages;
            if (RolePages.TryGetValue(role, out pages))
                foreach (string p in pages)
                    if (string.Equals(p, page, StringComparison.OrdinalIgnoreCase))
                        return true;

            return false;
        }
    }
}