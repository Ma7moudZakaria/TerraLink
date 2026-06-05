using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using static TerraLink.UseCase.IdentityShield.Features.Users.Operations.UserOperationHelpers;
using TerraLink.UseCase.IdentityShield.Features.Users.Endpoints;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Operations;

public sealed partial class UpdateUserOperation(
    UserManager<UserEntity> userManager,
    IRepository<RoleEntity> roles)
    : IOperationHandler<UpdateUserOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateUserOperation.Request request, CancellationToken ct = default)
    {
        UserEntity? user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        ErrorOr<List<RoleEntity>> rolesResult = await ValidateUpdateRequestAsync(userManager, roles, request.UserId, request.Payload, ct);
        if (rolesResult.IsError)
        {
            return rolesResult.FirstError;
        }

        string? email = NormalizeOptional(request.Payload.Email);
        string? phone = NormalizeOptional(request.Payload.Phone);

        user.Name = request.Payload.Name.Trim();
        user.UserName = request.Payload.UserName.Trim();
        user.Email = email;
        user.Phone = phone;
        user.PhoneNumber = phone;
        user.IsActive = request.Payload.IsActive;

        IdentityResult updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return Error.Validation("User.Update", string.Join(", ", updateResult.Errors.Select(error => error.Description)));
        }

        if (request.Payload.RoleIds is not null)
        {
            IdentityResult syncRolesResult = await SyncUserRolesAsync(userManager, user, rolesResult.Value);
            if (!syncRolesResult.Succeeded)
            {
                return Error.Validation("User.Roles", string.Join(", ", syncRolesResult.Errors.Select(error => error.Description)));
            }
        }

        return new Success();
    }

    private static async Task<ErrorOr<List<RoleEntity>>> ValidateUpdateRequestAsync(
        UserManager<UserEntity> userManager,
        IRepository<RoleEntity> roles,
        Guid userId,
        UpdateUserEndpoint.Request request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Error.Validation("User.Name", "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.UserName))
        {
            return Error.Validation("User.UserName", "Username is required.");
        }

        UserEntity? existingUser = await userManager.FindByNameAsync(request.UserName.Trim());
        if (existingUser is not null && existingUser.Id != userId)
        {
            return Error.Conflict("User.UserName", "Username already exists.");
        }

        string? email = NormalizeOptional(request.Email);
        if (!string.IsNullOrWhiteSpace(email))
        {
            UserEntity? existingEmail = await userManager.FindByEmailAsync(email);
            if (existingEmail is not null && existingEmail.Id != userId)
            {
                return Error.Conflict("User.Email", "Email already exists.");
            }
        }

        return await ValidateRolesAsync(roles, request.RoleIds, ct);
    }

    private static async Task<IdentityResult> SyncUserRolesAsync(
        UserManager<UserEntity> userManager,
        UserEntity user,
        List<RoleEntity> requestedRoles)
    {
        IList<string> currentRoleNames = await userManager.GetRolesAsync(user);
        List<string> requestedRoleNames = requestedRoles.Select(role => role.Name!).ToList();

        List<string> rolesToRemove = currentRoleNames.Except(requestedRoleNames, StringComparer.OrdinalIgnoreCase).ToList();
        if (rolesToRemove.Count != 0)
        {
            IdentityResult removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                return removeResult;
            }
        }

        List<string> rolesToAdd = requestedRoleNames.Except(currentRoleNames, StringComparer.OrdinalIgnoreCase).ToList();
        if (rolesToAdd.Count != 0)
        {
            IdentityResult addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                return addResult;
            }
        }

        return IdentityResult.Success;
    }
}
