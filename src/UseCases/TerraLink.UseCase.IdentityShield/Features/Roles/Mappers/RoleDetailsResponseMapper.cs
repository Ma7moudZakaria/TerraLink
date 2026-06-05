using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Mappers;

public sealed class RoleDetailsResponseMapper : IMapHandler<RoleEntity, GetRoleByIdOperation.Response>
{
    public GetRoleByIdOperation.Response Handler(RoleEntity source)
    {
        return new GetRoleByIdOperation.Response
        {
            Id = source.Id,
            Name = source.Name ?? string.Empty,
            Description = source.Description,
            PermissionIds = source.RolePermissions?.Select(rolePermission => rolePermission.PermissionLookupItemId).ToList() ?? []
        };
    }
}
