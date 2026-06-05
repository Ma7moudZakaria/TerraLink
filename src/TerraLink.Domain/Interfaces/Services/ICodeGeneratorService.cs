using LowCodeHub.QueryableExtensions.Entities;

namespace TerraLink.Domain.Interfaces.Services
{
    /// <summary>
    /// Service for generating unique entity codes/numbers
    /// </summary>
    public interface ICodeGeneratorService
    {
        /// <summary>
        /// Generates a unique code for an entity in the format: {EntityName}-{Year}-{Count:D4}
        /// Example: BUILDING-2025-0001, Employee-2025-0042
        /// </summary>
        /// <typeparam name="TEntity">The entity type to generate code for</typeparam>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Generated unique code</returns>
        Task<string> GenerateCodeAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : TrackedBaseEntity<Guid>;

        /// <summary>
        /// Generates a unique code for an entity with custom prefix
        /// Example: CUST-2025-0001, EMP-2025-0042
        /// </summary>
        /// <typeparam name="TEntity">The entity type to generate code for</typeparam>
        /// <param name="prefix">Custom prefix (e.g., "CUST", "EMP")</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Generated unique code</returns>
        Task<string> GenerateCodeAsync<TEntity>(string prefix, CancellationToken cancellationToken = default) where TEntity : TrackedBaseEntity<Guid>;
    }
}
