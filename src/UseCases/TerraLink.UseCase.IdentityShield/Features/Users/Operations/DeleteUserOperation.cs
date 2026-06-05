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

public sealed partial class DeleteUserOperation(UserManager<UserEntity> userManager, ICurrentUserService currentUserService)
    : IOperationHandler<DeleteUserOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteUserOperation.Request request, CancellationToken ct = default)
    {
        if (currentUserService.UserId == request.UserId)
        {
            return Error.Conflict("User.Delete", "You cannot delete the currently authenticated user.");
        }

        UserEntity? user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        IList<string> currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Count != 0)
        {
            IdentityResult removeRolesResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                return Error.Validation("User.Delete", string.Join(", ", removeRolesResult.Errors.Select(error => error.Description)));
            }
        }

        IdentityResult deleteResult = await userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
            return Error.Validation("User.Delete", string.Join(", ", deleteResult.Errors.Select(error => error.Description)));
        }

        return new Success();
    }
}
