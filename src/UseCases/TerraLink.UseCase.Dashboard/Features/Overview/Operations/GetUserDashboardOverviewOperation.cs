using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Dashboard.Features.Overview.Specifications;

namespace TerraLink.UseCase.Dashboard.Features.Overview.Operations;

public sealed partial class GetUserDashboardOverviewOperation(
    IRepository<RoleEntity> roles,
    IRepository<UserEntity> users,
    IRepository<UserRoleEntity> userRoles)
    : IOperationHandler<GetUserDashboardOverviewOperation.Request, GetUserDashboardOverviewOperation.Response>
{
    public async Task<ErrorOr<GetUserDashboardOverviewOperation.Response>> HandleAsync(GetUserDashboardOverviewOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<RoleEntity> roleEntities = await roles.ListAsync(ct);
        IReadOnlyList<UserEntity> userEntities = await users.ListAsync(ct);
        IReadOnlyList<UserRoleEntity> userRoleEntities = await userRoles.ListAsync(new DashboardUserRolesWithRoleSpec(), ct);

        List<GetUserDashboardOverviewOperation.RoleDistributionResponse> roleDistribution = userRoleEntities
            .Where(userRole => userRole.Role.Name is not null)
            .GroupBy(userRole => new { userRole.RoleId, userRole.Role.Name })
            .OrderByDescending(group => group.Select(userRole => userRole.UserId).Distinct().Count())
            .Select(group => new GetUserDashboardOverviewOperation.RoleDistributionResponse(
                group.Key.Name!,
                group.Select(userRole => userRole.UserId).Distinct().Count()))
            .ToList();

        return new GetUserDashboardOverviewOperation.Response(
            roleEntities.Count,
            userEntities.Count,
            0,
            userEntities.Count(user => user.IsActive == false),
            roleDistribution);
    }
}
