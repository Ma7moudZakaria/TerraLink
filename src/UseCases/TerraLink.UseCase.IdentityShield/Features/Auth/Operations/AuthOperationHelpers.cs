using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;
using TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

internal static class AuthOperationHelpers
{
    public static (Guid? RoleId, string? RoleName) GetPrimaryRole(RoleManager<RoleEntity> roleManager, Guid userId)
    {
        var roleResult = roleManager.Roles
            .SelectMany(role => role.UserRoles)
            .Where(userRole => userRole.UserId == userId)
            .Select(userRole => new
            {
                userRole.RoleId,
                RoleName = userRole.Role.Name
            })
            .FirstOrDefault();

        return roleResult is null
            ? (null, null)
            : (roleResult.RoleId, roleResult.RoleName);
    }
}
