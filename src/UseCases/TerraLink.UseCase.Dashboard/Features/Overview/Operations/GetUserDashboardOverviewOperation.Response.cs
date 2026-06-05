namespace TerraLink.UseCase.Dashboard.Features.Overview.Operations;

public sealed partial class GetUserDashboardOverviewOperation
{
    public sealed record RoleDistributionResponse(string RoleName, int UsersCount);

    public sealed record Response(
        int TotalRoles,
        int TotalUsers,
        int ActiveUsersToday,
        int InactiveUsers,
        List<RoleDistributionResponse> RoleDistribution);
}
