using System;
using System.Collections.Generic;

namespace MultiGroupSystemsTester.Admin
{
   
    public static class AdminAccess
    {
     
        private static readonly string[] FullAccessRoles =
        {
            "Manager",
            "Owner",
            "Tester"
        };

        
        private static readonly Dictionary<string, string[]> RolePages =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "System Administrator", new[] { "Dashboard", "AddProduct", "OrderLogistics", "ViewOrders", "AddSupplier" } }
            
        };

        public static bool CanAccess(string role, string page)
        {
            
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