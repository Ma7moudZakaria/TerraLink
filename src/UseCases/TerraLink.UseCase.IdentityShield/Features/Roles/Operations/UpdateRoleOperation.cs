using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class UpdateRoleOperation(
    IRepository<RoleEntity> roles,
    IRepository<RolePermissionEntity> rolePermissions,
    ITransactionManager<TerraLinkDbContext> transactionManager)
    : IOperationHandler<UpdateRoleOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateRoleOperation.Request request, CancellationToken ct = default)
    {
        RoleEntity? role = await roles.GetAsync(new RoleByIdSpec(request.RoleId), ct);
        if (role is null)
        {
            return Error.NotFound("Role.NotFound", "Role not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Payload.Name))
        {
            return Error.Validation("Role.Name", "Role name is required.");
        }

        string name = request.Payload.Name.Trim();
        if (await roles.CountAsync(new RoleByNameSpec(name, request.RoleId), ct) > 0)
        {
            return Error.Conflict("Role.Name", "A role with this name already exists.");
        }

        await transactionManager.ExecuteInTransactionAsync(async _ =>
        {
            await roles.UpdateAsync(new RoleByIdSpec(request.RoleId), new UpdateRoleSpec(name, request.Payload.Description), ct);
            await rolePermissions.RemoveAsync(new RolePermissionsByRoleIdSpec(request.RoleId), ct);

            if (request.Payload.PermissionIds.Count > 0)
            {
                rolePermissions.AddRange(new AddRolePermissionsSpec(request.RoleId, request.Payload.PermissionIds));
                await rolePermissions.SaveChangesAsync(ct);
            }
        }, ct);

        return new Success();
    }
}
