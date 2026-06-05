namespace TerraLink.UseCase.Dashboard.Features.Overview.Endpoints;

public sealed partial class GetUserDashboardOverviewEndpoint
{
    public sealed class RoleDistributionResponse
    {
        public string RoleName { get; set; } = string.Empty;
        public int UsersCount { get; set; }
    }

    public sealed class Response
    {
        public int TotalRoles { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsersToday { get; set; }
        public int InactiveUsers { get; set; }
        public List<RoleDistributionResponse> RoleDistribution { get; set; } = [];
    }
}
