using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TerraLink.Domain.Interfaces;
using TerraLink.Domain.Persistence;

namespace TerraLink.API.Services
{
    /// <summary>
    /// Service for accessing the current authenticated user's information
    /// </summary>
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor, TerraLinkDbContext dbContext, IDistributedCache cache) : ICurrentUserService
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
        private const string CacheKeyPrefix = "perms:";

        public Guid? UserId
        {
            get
            {
                // Use "sub" claim type (JWT standard) instead of ClaimTypes.NameIdentifier
                var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst("sub");

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }

                return null;
            }
        }

        // Use "unique_name" claim type instead of ClaimTypes.Name
        public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirst("unique_name")?.Value;

        // Use "email" claim type instead of ClaimTypes.Email
        public string? Email => httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;

        public IEnumerable<string> Roles => httpContextAccessor.HttpContext?.User?.FindAll("roles").Select(c => c.Value) ?? [];

        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public async Task<bool> HasPermissionAsync(string permission, CancellationToken cancellationToken = default)
        {
            if (!IsAuthenticated || UserId == null || !Roles.Any())
            {
                return false;
            }

            HashSet<string> permissions = await GetCachedPermissionsAsync(cancellationToken);
            return permissions.Contains(permission);
        }

        private async Task<HashSet<string>> GetCachedPermissionsAsync(CancellationToken cancellationToken)
        {
            string cacheKey = $"{CacheKeyPrefix}{UserId}";

            byte[]? cached = await cache.GetAsync(cacheKey, cancellationToken);
            if (cached is not null)
            {
                return JsonSerializer.Deserialize<HashSet<string>>(cached) ?? [];
            }

            List<string> permissionCodes = await dbContext.RolePermissions
                .Where(a => Roles.Contains(a.Role.Name))
                .Select(a => a.PermissionLookupItem.Code)
                .Distinct()
                .ToListAsync(cancellationToken);

            HashSet<string> permissionSet = new(permissionCodes, StringComparer.OrdinalIgnoreCase);

            await cache.SetAsync(cacheKey, JsonSerializer.SerializeToUtf8Bytes(permissionSet), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheDuration
            }, cancellationToken);

            return permissionSet;
        }
    }
}
