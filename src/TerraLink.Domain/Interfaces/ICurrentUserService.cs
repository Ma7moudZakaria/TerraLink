using LowCodeHub.Permissions;

namespace TerraLink.Domain.Interfaces
{
    /// <summary>
    /// Service for accessing the current authenticated user's information.
    /// Extends <see cref="IPermissionService"/> for permission-based authorization.
    /// </summary>
    public interface ICurrentUserService : IPermissionService
    {
        /// <summary>
        /// Gets the current user's ID
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// Gets the current user's username
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets the current user's email
        /// </summary>
        string? Email { get; }

        /// <summary>
        /// Gets the current user's roles
        /// </summary>
        IEnumerable<string> Roles { get; }
    }
}
