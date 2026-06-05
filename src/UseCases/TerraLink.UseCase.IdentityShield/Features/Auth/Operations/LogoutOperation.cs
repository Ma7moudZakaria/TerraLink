using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

public sealed partial class LogoutOperation(IJwtTokenProvider tokenProvider)
    : IOperationHandler<LogoutOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(LogoutOperation.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Error.Validation("RefreshToken", "Refresh token is required.");
        }

        await tokenProvider.RevokeRefreshTokenAsync(request.RefreshToken, ct);
        return new Success();
    }
}
