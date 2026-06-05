using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Roles.Mappers;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class GetRoleDashboardOperation(
    IRepository<RoleEntity> roles,
    IRepository<UserEntity> users,
    IRepository<UserRoleEntity> userRoles,
    IMapper mapper)
    : IOperationHandler<GetRoleDashboardOperation.Request, GetRoleDashboardOperation.Response>
{
    public async Task<ErrorOr<GetRoleDashboardOperation.Response>> HandleAsync(GetRoleDashboardOperation.Request request, CancellationToken ct = default)
    {
        int totalRoles = await roles.CountAsync(new RolesOrderedSpec(), ct);
        int totalUsers = await users.CountAsync(new AllUsersSpec(), ct);
        int activeUsers = await users.CountAsync(new ActiveUsersSpec(), ct);
        int inactiveUsers = await users.CountAsync(new InactiveUsersSpec(), ct);

        IReadOnlyList<RoleEntity> roleList = await roles.ListAsync(new RolesOrderedSpec(), ct);
        List<Guid> roleIds = roleList.Select(role => role.Id).ToList();
        IReadOnlyList<UserRoleEntity> roleAssignments = roleIds.Count == 0
            ? []
            : await userRoles.ListAsync(new UserRoleByRoleIdsSpec(roleIds), ct);

        Dictionary<Guid, int> userCountByRole = roleAssignments
            .GroupBy(userRole => userRole.RoleId)
            .ToDictionary(group => group.Key, group => group.Count());

        return new GetRoleDashboardOperation.Response
        {
            TotalRoles = totalRoles,
            TotalUsers = totalUsers,
            ActiveUsers = activeUsers,
            InactiveUsers = inactiveUsers,
            RoleDistribution = roleList
                .Select(role => new RoleDistributionResponseMapper.Source(role, userCountByRole.GetValueOrDefault(role.Id)))
                .Select(mapper.Map<RoleDistributionResponseMapper.Source, GetRoleDashboardOperation.RoleDistributionResponse>)
                .ToList()
        };
    }
}
