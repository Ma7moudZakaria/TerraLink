using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Models;

namespace TerraLink.UseCase.IdentityShield.Abstracts.Shield
{
    public interface IJwtTokenProvider
    {
        Task<JwtResponse> GetJwtAsync(UserEntity user, CancellationToken cancellationToken = default);
        Task<(bool IsValid, Guid UserId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
