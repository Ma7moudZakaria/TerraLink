using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class DeleteRoleOperation(
    IRepository<RoleEntity> roles,
    IRepository<RolePermissionEntity> rolePermissions,
    ITransactionManager<TerraLinkDbContext> transactionManager)
    : IOperationHandler<DeleteRoleOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteRoleOperation.Request request, CancellationToken ct = default)
    {
        RoleEntity? role = await roles.GetAsync(new RoleByIdSpec(request.RoleId, includeUsers: true), ct);
        if (role is null)
        {
            return Error.NotFound("Role.NotFound", "Role not found.");
        }

        if (role.UserRoles?.Any() == true)
        {
            return Error.Conflict("Role.HasUsers", "Cannot delete a role that has assigned users.");
        }

        await transactionManager.ExecuteInTransactionAsync(async _ =>
        {
            await rolePermissions.RemoveAsync(new RolePermissionsByRoleIdSpec(request.RoleId), ct);
            await roles.RemoveAsync(new RoleByIdSpec(request.RoleId), ct);
        }, ct);

        return new Success();
    }
}
