using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using LowCodeHub.QueryableExtensions.Abstractions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;
using TerraLink.UseCase.IdentityShield.Features.Auth.Specifications;
using TerraLink.UseCase.IdentityShield.Models;


namespace TerraLink.UseCase.IdentityShield.Implementation.Shield
{
    public sealed class JwtTokenProvider(
        IOptions<IdentityShieldSettings> _identitySettings,
        IIdentityClaimMapper<UserEntity> claimMapper,
        IRepository<RefreshTokenEntity> refreshTokens) : IJwtTokenProvider
    {
        private readonly IdentityShieldSettings _settings = _identitySettings.Value;

        public async Task<JwtResponse> GetJwtAsync(UserEntity user, CancellationToken cancellationToken = default)
        {
            // Implement single session logic if enabled
            if (_settings.Session.EnableSingleSession)
            {
                await RevokeAllUserRefreshTokensAsync(user.Id, cancellationToken);
            }
            // Implement max concurrent sessions if configured
            else if (_settings.Session.MaxConcurrentSessions > 0)
            {
                await EnforceMaxConcurrentSessionsAsync(user.Id, cancellationToken);
            }

            return new JwtResponse
            {
                RefreshToken = await GenerateRefreshTokenAsync(user.Id, cancellationToken),
                AccessToken = await GenerateAccessTokenAsync(user, cancellationToken)
            };
        }

        private async Task<string> GenerateAccessTokenAsync(UserEntity user, CancellationToken cancellationToken)
        {
            List<Claim> claims = await claimMapper.GetClaims(user).ToListAsync(cancellationToken);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_settings.Jwt.AccessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.Secret)), SecurityAlgorithms.HmacSha256),
                Issuer = _settings.Jwt.Issuer,
                Audience = _settings.Jwt.Audience,
                IssuedAt = DateTime.UtcNow,
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }

        private async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            static string GenerateRefreshToken()
            {
                byte[] randomBytes = new byte[64];

                using var rng = RandomNumberGenerator.Create();

                rng.GetBytes(randomBytes);

                return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").TrimEnd('=');
            }

            DateTime now = DateTime.UtcNow;
            string token = GenerateRefreshToken();

            refreshTokens.Add(new CreateRefreshTokenSpec(
                Guid.NewGuid(),
                userId,
                token,
                now.AddDays(_settings.Jwt.RefreshTokenExpirationDays),
                now));

            await refreshTokens.SaveChangesAsync(cancellationToken);

            return token;
        }

        public async Task<(bool IsValid, Guid UserId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            RefreshTokenEntity? token = await refreshTokens.GetAsync(new RefreshTokenByTokenSpec(refreshToken), cancellationToken);

            if (token == null)
                return (false, Guid.Empty);

            if (token.IsRevoked)
                return (false, Guid.Empty);

            if (token.ExpiresAt < DateTime.UtcNow)
            {
                await RevokeRefreshTokenAsync(token.Token, cancellationToken);

                return (false, Guid.Empty);
            }

            return (true, token.UserId);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            await refreshTokens.UpdateAsync(
                new ActiveRefreshTokenByTokenSpec(refreshToken),
                new RevokeRefreshTokenSpec(DateTime.UtcNow),
                cancellationToken);
        }

        /// <summary>
        /// Revokes all active refresh tokens for a user (Single Session Mode)
        /// </summary>
        private async Task RevokeAllUserRefreshTokensAsync(Guid userId, CancellationToken cancellationToken)
        {
            await refreshTokens.UpdateAsync(
                new ActiveRefreshTokensByUserSpec(userId),
                new RevokeRefreshTokenSpec(DateTime.UtcNow),
                cancellationToken);
        }

        /// <summary>
        /// Enforces maximum concurrent sessions by revoking oldest tokens
        /// </summary>
        private async Task EnforceMaxConcurrentSessionsAsync(Guid userId, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            IReadOnlyList<RefreshTokenEntity> activeTokens = await refreshTokens.ListAsync(
                new ActiveUnexpiredRefreshTokensByUserSpec(userId, now),
                cancellationToken);

            int tokensToRevoke = activeTokens.Count - _settings.Session.MaxConcurrentSessions + 1;

            if (tokensToRevoke > 0)
            {
                List<Guid> tokenIdsToRevoke = activeTokens
                    .Take(tokensToRevoke)
                    .Select(token => token.Id)
                    .ToList();

                await refreshTokens.UpdateAsync(
                    new RefreshTokensByIdsSpec(tokenIdsToRevoke),
                    new RevokeRefreshTokenSpec(now),
                    cancellationToken);
            }
        }
    }
}
