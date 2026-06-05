using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class GetRolesOperation(IRepository<RoleEntity> roles, IMapper mapper)
    : IOperationHandler<GetRolesOperation.Request, List<GetRolesOperation.Response>>
{
    public async Task<ErrorOr<List<GetRolesOperation.Response>>> HandleAsync(GetRolesOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<RoleEntity> roleList = await roles.ListAsync(new RolesOrderedSpec(includeUsers: true), ct);

        return roleList.Select(mapper.Map<RoleEntity, GetRolesOperation.Response>).ToList();
    }
}
