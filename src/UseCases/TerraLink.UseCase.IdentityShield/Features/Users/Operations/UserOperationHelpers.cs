using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using static TerraLink.UseCase.IdentityShield.Features.Users.Operations.UserOperationHelpers;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Operations;

internal static class UserOperationHelpers
{
    public static async Task<ErrorOr<List<RoleEntity>>> ValidateRolesAsync(
        IRepository<RoleEntity> roles,
        List<Guid>? roleIds,
        CancellationToken ct)
    {
        if (roleIds is null || roleIds.Count == 0)
        {
            return new List<RoleEntity>();
        }

        List<Guid> distinctRoleIds = roleIds.Where(roleId => roleId != Guid.Empty).Distinct().ToList();
        IReadOnlyList<RoleEntity> existingRoles = await roles.ListAsync(new RolesByIdsSpec(distinctRoleIds), ct);

        if (existingRoles.Count != distinctRoleIds.Count)
        {
            return Error.Validation("User.RoleIds", "One or more supplied roles do not exist.");
        }

        return existingRoles.ToList();
    }

    public static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

}
