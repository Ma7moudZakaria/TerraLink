using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Specifications;

public sealed class CreateRefreshTokenSpec(
    Guid id,
    Guid userId,
    string token,
    DateTime expiresAt,
    DateTime createdAt) : IAddSpecification<RefreshTokenEntity>
{
    public RefreshTokenEntity Add()
    {
        return new RefreshTokenEntity
        {
            Id = id,
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = createdAt,
            IsRevoked = false
        };
    }
}

public sealed class RefreshTokenByTokenSpec(string token) : ISpecification<RefreshTokenEntity>
{
    public IQueryable<RefreshTokenEntity> Where(IQueryable<RefreshTokenEntity> query)
        => query.Where(entity => entity.Token == token);
}

public sealed class ActiveRefreshTokenByTokenSpec(string token) : ISpecification<RefreshTokenEntity>
{
    public IQueryable<RefreshTokenEntity> Where(IQueryable<RefreshTokenEntity> query)
        => query.Where(entity => entity.Token == token && !entity.IsRevoked);
}

public sealed class ActiveRefreshTokensByUserSpec(Guid userId) : ISpecification<RefreshTokenEntity>
{
    public IQueryable<RefreshTokenEntity> Where(IQueryable<RefreshTokenEntity> query)
        => query.Where(entity => entity.UserId == userId && !entity.IsRevoked);
}

public sealed class ActiveUnexpiredRefreshTokensByUserSpec(Guid userId, DateTime now) : ISpecification<RefreshTokenEntity>
{
    public IQueryable<RefreshTokenEntity> Where(IQueryable<RefreshTokenEntity> query)
        => query
            .Where(entity => entity.UserId == userId && !entity.IsRevoked && entity.ExpiresAt > now)
            .OrderBy(entity => entity.CreatedAt);
}

public sealed class RefreshTokensByIdsSpec(IReadOnlyCollection<Guid> tokenIds) : ISpecification<RefreshTokenEntity>
{
    public IQueryable<RefreshTokenEntity> Where(IQueryable<RefreshTokenEntity> query)
        => query.Where(entity => tokenIds.Contains(entity.Id));
}

public sealed class RevokeRefreshTokenSpec(DateTime revokedAt) : IUpdateSpecification<RefreshTokenEntity>
{
    public Action<UpdateSettersBuilder<RefreshTokenEntity>> Update()
    {
        return setters => setters
            .SetProperty(entity => entity.IsRevoked, true)
            .SetProperty(entity => entity.RevokedAt, revokedAt);
    }
}
