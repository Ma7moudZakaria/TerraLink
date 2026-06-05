using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TerraLink.UseCase.IdentityShield.Abstracts.Shield
{
    public interface IIdentityClaimMapper<in TUser> where TUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets the claims for the given user.
        /// </summary>
        /// <param name="user">The user to extract claims from.</param>
        /// <returns>A collection of claims to be added to the token.</returns>
        IAsyncEnumerable<Claim> GetClaims(TUser user);
    }
}
