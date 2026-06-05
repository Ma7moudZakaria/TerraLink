using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Mappers;

public sealed class RoleListResponseMapper : IMapHandler<RoleEntity, GetRolesOperation.Response>
{
    public GetRolesOperation.Response Handler(RoleEntity source)
    {
        return new GetRolesOperation.Response
        {
            Id = source.Id,
            Name = source.Name ?? string.Empty,
            Description = source.Description,
            UserCount = source.UserRoles?.Count ?? 0
        };
    }
}
