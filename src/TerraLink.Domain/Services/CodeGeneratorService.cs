using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Entities;
using LowCodeHub.QueryableExtensions.Specifications;
using TerraLink.Domain.Interfaces.Services;

namespace TerraLink.Domain.Services
{
    /// <summary>
    /// Service for generating unique entity codes/numbers.
    /// Format: {EntityName}-{Year}-{Count:D4}
    /// </summary>
    public class CodeGeneratorService(IServiceProvider services) : ICodeGeneratorService
    {
        public async Task<string> GenerateCodeAsync<TEntity>(CancellationToken cancellationToken = default)
            where TEntity : TrackedBaseEntity<Guid>
        {
            string entityName = GetEntityPrefix<TEntity>();

            return await GenerateCodeAsync<TEntity>(entityName, cancellationToken);
        }

        public async Task<string> GenerateCodeAsync<TEntity>(string prefix, CancellationToken cancellationToken = default)
            where TEntity : TrackedBaseEntity<Guid>
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be null or empty", nameof(prefix));
            }

            object? repository = services.GetService(typeof(IRepository<TEntity>));
            if (repository is not IRepository<TEntity> typedRepository)
            {
                throw new InvalidOperationException($"Generic repository for '{typeof(TEntity).Name}' is not registered.");
            }

            int year = DateTime.UtcNow.Year;
            int nextNumber = await typedRepository.CountAsync(new EntityCreatedInYearSpec<TEntity>(year), cancellationToken) + 1;

            if (nextNumber > 9999)
            {
                throw new InvalidOperationException($"Maximum sequence number (9999) exceeded for {prefix} in year {year}. Consider using a different code format or archiving old records.");
            }

            return $"{prefix.ToUpperInvariant()}-{year}-{nextNumber:D4}";
        }

        private static string GetEntityPrefix<TEntity>() where TEntity : class
        {
            string typeName = typeof(TEntity).Name;

            if (typeName.EndsWith("Entity", StringComparison.OrdinalIgnoreCase))
            {
                typeName = typeName[..^6];
            }

            return typeName.ToUpperInvariant();
        }

        private sealed class EntityCreatedInYearSpec<TEntity>(int year) : ISpecification<TEntity>
            where TEntity : TrackedBaseEntity<Guid>
        {
            public IQueryable<TEntity> Where(IQueryable<TEntity> query)
            {
                DateTime startDate = new(year, 1, 1);
                DateTime endDate = startDate.AddYears(1);

                return query.Where(entity => entity.CreatedDate >= startDate && entity.CreatedDate < endDate);
            }
        }
    }
}
