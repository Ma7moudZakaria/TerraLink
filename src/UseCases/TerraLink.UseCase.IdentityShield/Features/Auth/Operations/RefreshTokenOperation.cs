using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

public sealed partial class RefreshTokenOperation(
    UserManager<UserEntity> userManager,
    IJwtTokenProvider tokenProvider)
    : IOperationHandler<RefreshTokenOperation.Request, RefreshTokenOperation.Response>
{
    public async Task<ErrorOr<RefreshTokenOperation.Response>> HandleAsync(RefreshTokenOperation.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Error.Validation("RefreshToken", "Refresh token is required.");
        }

        (bool isValid, Guid userId) = await tokenProvider.ValidateRefreshTokenAsync(request.RefreshToken, ct);
        if (!isValid)
        {
            return Error.Unauthorized("RefreshToken", "Invalid or expired refresh token.");
        }

        UserEntity? user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Error.NotFound("RefreshToken", "User not found.");
        }

        if (user.IsActive == false)
        {
            return Error.Forbidden("RefreshToken", "User account is deactivated.");
        }

        await tokenProvider.RevokeRefreshTokenAsync(request.RefreshToken, ct);

        return new RefreshTokenOperation.Response
        {
            Succeeded = true,
            Message = "Token refreshed successfully.",
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = await tokenProvider.GetJwtAsync(user, ct)
        };
    }
}
