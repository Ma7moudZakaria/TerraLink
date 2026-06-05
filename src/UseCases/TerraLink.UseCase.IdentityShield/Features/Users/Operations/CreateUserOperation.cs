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

public sealed partial class CreateUserOperation(
    UserManager<UserEntity> userManager,
    IRepository<RoleEntity> roles)
    : IOperationHandler<CreateUserOperation.Request, CreateUserOperation.Response>
{
    public async Task<ErrorOr<CreateUserOperation.Response>> HandleAsync(CreateUserOperation.Request request, CancellationToken ct = default)
    {
        ErrorOr<List<RoleEntity>> rolesResult = await ValidateCreateRequestAsync(userManager, roles, request.Payload, ct);
        if (rolesResult.IsError)
        {
            return rolesResult.FirstError;
        }

        string? email = NormalizeOptional(request.Payload.Email);
        string? phone = NormalizeOptional(request.Payload.Phone);

        UserEntity user = new()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Payload.Name.Trim(),
            UserName = request.Payload.UserName.Trim(),
            Email = email,
            Phone = phone,
            PhoneNumber = phone,
            IsActive = request.Payload.IsActive
        };

        IdentityResult createResult = await userManager.CreateAsync(user, request.Payload.Password);
        if (!createResult.Succeeded)
        {
            return Error.Validation("User.Create", string.Join(", ", createResult.Errors.Select(error => error.Description)));
        }

        if (rolesResult.Value.Count != 0)
        {
            IdentityResult roleAssignmentResult = await userManager.AddToRolesAsync(user, rolesResult.Value.Select(role => role.Name!).ToList());
            if (!roleAssignmentResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return Error.Validation("User.Roles", string.Join(", ", roleAssignmentResult.Errors.Select(error => error.Description)));
            }
        }

        return new CreateUserOperation.Response { Id = user.Id };
    }

    private static async Task<ErrorOr<List<RoleEntity>>> ValidateCreateRequestAsync(
        UserManager<UserEntity> userManager,
        IRepository<RoleEntity> roles,
        CreateUserEndpoint.Request request,
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

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return Error.Validation("User.Password", "Password is required.");
        }

        if (request.Password != request.ConfirmPassword)
        {
            return Error.Validation("User.ConfirmPassword", "Passwords do not match.");
        }

        UserEntity? existingUser = await userManager.FindByNameAsync(request.UserName.Trim());
        if (existingUser is not null)
        {
            return Error.Conflict("User.UserName", "Username already exists.");
        }

        string? email = NormalizeOptional(request.Email);
        if (!string.IsNullOrWhiteSpace(email))
        {
            UserEntity? existingEmail = await userManager.FindByEmailAsync(email);
            if (existingEmail is not null)
            {
                return Error.Conflict("User.Email", "Email already exists.");
            }
        }

        return await ValidateRolesAsync(roles, request.RoleIds, ct);
    }
}
