namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations
{
    public sealed partial class GetRoleDashboardOperation
    {
        public sealed class Response
        {
            public int TotalRoles { get; set; }
            public int TotalUsers { get; set; }
            public int ActiveUsers { get; set; }
            public int InactiveUsers { get; set; }
            public List<RoleDistributionResponse> RoleDistribution { get; set; } = [];
        }

        public sealed class RoleDistributionResponse
        {
            public Guid RoleId { get; set; }
            public string RoleName { get; set; } = string.Empty;
            public int UserCount { get; set; }
        }
    }
}
