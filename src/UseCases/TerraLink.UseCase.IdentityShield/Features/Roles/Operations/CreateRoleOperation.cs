using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class CreateRoleOperation(IRepository<RoleEntity> roles)
    : IOperationHandler<CreateRoleOperation.Request, CreateRoleOperation.Response>
{
    public async Task<ErrorOr<CreateRoleOperation.Response>> HandleAsync(CreateRoleOperation.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Payload.Name))
        {
            return Error.Validation("Role.Name", "Role name is required.");
        }

        string name = request.Payload.Name.Trim();
        if (await roles.CountAsync(new RoleByNameSpec(name), ct) > 0)
        {
            return Error.Conflict("Role.Name", "A role with this name already exists.");
        }

        Guid roleId = Guid.CreateVersion7();
        roles.Add(new AddRoleSpec(roleId, name, request.Payload.Description, request.Payload.PermissionIds));

        return await roles.SaveChangesAsync(ct) > 0
            ? new CreateRoleOperation.Response { Id = roleId }
            : Error.Validation("Role.Create", "Role could not be created.");
    }
}
