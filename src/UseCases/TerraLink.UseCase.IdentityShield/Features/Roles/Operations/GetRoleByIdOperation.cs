using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class GetRoleByIdOperation(IRepository<RoleEntity> roles, IMapper mapper)
    : IOperationHandler<GetRoleByIdOperation.Request, GetRoleByIdOperation.Response>
{
    public async Task<ErrorOr<GetRoleByIdOperation.Response>> HandleAsync(GetRoleByIdOperation.Request request, CancellationToken ct = default)
    {
        RoleEntity? role = await roles.GetAsync(new RoleByIdSpec(request.RoleId, includePermissions: true), ct);
        if (role is null)
        {
            return Error.NotFound("Role.NotFound", "Role not found.");
        }

        return mapper.Map<RoleEntity, GetRoleByIdOperation.Response>(role);
    }
}
