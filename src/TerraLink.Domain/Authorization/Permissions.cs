namespace TerraLink.Domain.Authorization
{
    /// <summary>
    /// Static class containing all application permissions organized by module
    /// </summary>
    public static class Permissions
    {
        // =============================================
        // ASSETS MODULE - Controls Buildings, Lands, Units
        // =============================================
        public static class Assets
        {
            public const string View = "Permissions.Assets.View";
            public const string Create = "Permissions.Assets.Create";
            public const string Edit = "Permissions.Assets.Edit";
            public const string Delete = "Permissions.Assets.Delete";
        }

        // =============================================
        // CLIENTS MODULE
        // =============================================
        public static class Clients
        {
            public const string View = "Permissions.Clients.View";
            public const string Create = "Permissions.Clients.Create";
            public const string Edit = "Permissions.Clients.Edit";
            public const string Delete = "Permissions.Clients.Delete";
        }

        // =============================================
        // CONTRACTS MODULE
        // =============================================
        public static class Contracts
        {
            public const string View = "Permissions.Contracts.View";
            public const string Create = "Permissions.Contracts.Create";
            public const string Edit = "Permissions.Contracts.Edit";
            public const string Delete = "Permissions.Contracts.Delete";
        }

        // =============================================
        // FINANCE MODULE - Controls Income/Outgoing Payments
        // =============================================
        public static class Finance
        {
            public const string View = "Permissions.Finance.View";
            public const string Create = "Permissions.Finance.Create";
            public const string Edit = "Permissions.Finance.Edit";
            public const string Delete = "Permissions.Finance.Delete";
        }

        // =============================================
        // FollowUpRecords MODULE
        // =============================================
        public static class FollowUpRecords
        {
            public const string View = "Permissions.FollowUpRecords.View";
            public const string Create = "Permissions.FollowUpRecords.Create";
            public const string Edit = "Permissions.FollowUpRecords.Edit";
            public const string Delete = "Permissions.FollowUpRecords.Delete";
        }


        // =============================================
        // IDENTITY MODULE - Admin Only (Users, Roles management)
        // =============================================
        public static class Identity
        {
            public const string View = "Permissions.Identity.View";
            public const string Create = "Permissions.Identity.Create";
            public const string Edit = "Permissions.Identity.Edit";
            public const string Delete = "Permissions.Identity.Delete";
        }

        // =============================================
        // DOCUMENTS/ATTACHMENTS MODULE
        // Note: Upload/Download only require authentication (no specific permissions)
        // View/Delete require permissions for administrative control
        // =============================================
        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Upload = "Permissions.Documents.Upload";
            public const string Download = "Permissions.Documents.Download";
            public const string Delete = "Permissions.Documents.Delete";
        }

        // =============================================
        // LOOKUPS MODULE
        // Note: View requires authentication only (no specific permission)
        // Create/Edit/Delete require permissions for administrative control
        // =============================================
        public static class Lookups
        {
            public const string View = "Permissions.Lookups.View";
            public const string Create = "Permissions.Lookups.Create";
            public const string Edit = "Permissions.Lookups.Edit";
            public const string Delete = "Permissions.Lookups.Delete";
        }

        // =============================================
        // DASHBOARD & REPORTS
        // =============================================
        public static class Dashboard
        {
            public const string View = "Permissions.Dashboard.View";
            public const string ViewFinancialReports = "Permissions.Dashboard.ViewFinancialReports";
            public const string ViewAnalytics = "Permissions.Dashboard.ViewAnalytics";
        }

        /// <summary>
        /// Get all permissions as a list
        /// </summary>
        public static List<string> GetAllPermissions()
        {
            return new List<string>
            {
                // Assets Module (Buildings, Lands, Units)
                Assets.View, Assets.Create, Assets.Edit, Assets.Delete,
                
                // Clients Module
                Clients.View, Clients.Create, Clients.Edit, Clients.Delete,
                
                // Contracts Module
                Contracts.View, Contracts.Create, Contracts.Edit, Contracts.Delete,
                
                // Finance Module (Income/Outgoing Payments)
                Finance.View, Finance.Create, Finance.Edit, Finance.Delete,

                // Follow-up Calls Module
                FollowUpRecords.View, FollowUpRecords.Create, FollowUpRecords.Edit, FollowUpRecords.Delete,
                
                // Identity Module (Admin Only)
                Identity.View, Identity.Create, Identity.Edit, Identity.Delete,
                
                // Documents/Attachments Module
                Documents.View, Documents.Upload, Documents.Download, Documents.Delete,
                
                // Lookups Module
                Lookups.View, Lookups.Create, Lookups.Edit, Lookups.Delete,
                
                // Dashboard
                Dashboard.View, Dashboard.ViewFinancialReports, Dashboard.ViewAnalytics
            };
        }

        /// <summary>
        /// Get all permissions grouped by module
        /// </summary>
        public static Dictionary<string, List<string>> GetPermissionsByModule()
        {
            return new Dictionary<string, List<string>>
            {
                ["Assets"] = new List<string> { Assets.View, Assets.Create, Assets.Edit, Assets.Delete },
                ["Clients"] = new List<string> { Clients.View, Clients.Create, Clients.Edit, Clients.Delete },
                ["Contracts"] = new List<string> { Contracts.View, Contracts.Create, Contracts.Edit, Contracts.Delete },
                ["Finance"] = new List<string> { Finance.View, Finance.Create, Finance.Edit, Finance.Delete },
                ["FollowUpRecords"] = new List<string> { FollowUpRecords.View, FollowUpRecords.Create, FollowUpRecords.Edit, FollowUpRecords.Delete },
                ["Identity"] = new List<string> { Identity.View, Identity.Create, Identity.Edit, Identity.Delete },
                ["Documents"] = new List<string> { Documents.View, Documents.Upload, Documents.Download, Documents.Delete },
                ["Lookups"] = new List<string> { Lookups.View, Lookups.Create, Lookups.Edit, Lookups.Delete },
                ["Dashboard"] = new List<string> { Dashboard.View, Dashboard.ViewFinancialReports, Dashboard.ViewAnalytics }
            };
        }
    }
}
