using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;
using TerraLink.UseCase.IdentityShield.Models;

namespace TerraLink.UseCase.IdentityShield.Implementation.Shield
{
    public class ShieldIdentityClaimMapper(IOptions<IdentityShieldSettings> _shieldSettings, UserManager<UserEntity> _userManager) : IIdentityClaimMapper<UserEntity>
    {
        public async IAsyncEnumerable<Claim> GetClaims(UserEntity user)
        {
            ArgumentNullException.ThrowIfNull(user);

            yield return new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            yield return new(JwtRegisteredClaimNames.Aud, _shieldSettings.Value.Jwt.Audience);
            yield return new(JwtRegisteredClaimNames.Sub, user.Id.ToString());
            yield return new(JwtRegisteredClaimNames.UniqueName, user.UserName!);
            yield return new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty);
            yield return new(JwtRegisteredClaimNames.Name, user.Name);
            yield return new("phone", user.Phone ?? string.Empty);


            // Add other custom claims here.
            if (!string.IsNullOrEmpty(user.UserName))
            {
                yield return new Claim("username", user.UserName);
            }

            foreach (string role in await _userManager.GetRolesAsync(user))
            {
                yield return new Claim("roles", role);
            }
        }
    }
}
