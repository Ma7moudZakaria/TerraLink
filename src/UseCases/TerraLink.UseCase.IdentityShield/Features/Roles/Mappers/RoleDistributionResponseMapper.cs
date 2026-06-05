using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Mappers;

public sealed class RoleDistributionResponseMapper
    : IMapHandler<RoleDistributionResponseMapper.Source, GetRoleDashboardOperation.RoleDistributionResponse>
{
    public sealed record Source(RoleEntity Role, int UserCount);

    public GetRoleDashboardOperation.RoleDistributionResponse Handler(Source source)
    {
        return new GetRoleDashboardOperation.RoleDistributionResponse
        {
            RoleId = source.Role.Id,
            RoleName = source.Role.Name ?? string.Empty,
            UserCount = source.UserCount
        };
    }
}
