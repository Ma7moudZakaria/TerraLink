using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Specifications;

public sealed class AllUsersSpec : ISpecification<UserEntity>
{
    public IQueryable<UserEntity> Where(IQueryable<UserEntity> query)
    {
        return query;
    }
}

public sealed class ActiveUsersSpec : ISpecification<UserEntity>
{
    public IQueryable<UserEntity> Where(IQueryable<UserEntity> query)
    {
        return query.Where(user => user.IsActive == true);
    }
}

public sealed class InactiveUsersSpec : ISpecification<UserEntity>
{
    public IQueryable<UserEntity> Where(IQueryable<UserEntity> query)
    {
        return query.Where(user => user.IsActive == false || user.IsActive == null);
    }
}

public sealed class UserByIdSpec(Guid id) : ISpecification<UserEntity>
{
    public IQueryable<UserEntity> Where(IQueryable<UserEntity> query)
    {
        return query
            .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role)
            .Where(user => user.Id == id);
    }
}

public sealed class UsersListSpec(
    string? searchTerm,
    string? userName,
    string? name,
    string? email,
    string? phone,
    Guid? roleId,
    bool? isActive) : ISpecification<UserEntity>
{
    public IQueryable<UserEntity> Where(IQueryable<UserEntity> query)
    {
        query = query
            .Include(user => user.UserRoles)
                .ThenInclude(userRole => userRole.Role);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string term = searchTerm.Trim();
            query = query.Where(user =>
                user.Name.Contains(term) ||
                (user.UserName != null && user.UserName.Contains(term)) ||
                (user.Email != null && user.Email.Contains(term)) ||
                (user.Phone != null && user.Phone.Contains(term)) ||
                (user.PhoneNumber != null && user.PhoneNumber.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            query = query.Where(user => user.UserName != null && user.UserName.Contains(userName));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(user => user.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(user => user.Email != null && user.Email.Contains(email));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(user =>
                (user.Phone != null && user.Phone.Contains(phone)) ||
                (user.PhoneNumber != null && user.PhoneNumber.Contains(phone)));
        }

        if (roleId.HasValue)
        {
            query = query.Where(user => user.UserRoles.Any(userRole => userRole.RoleId == roleId.Value));
        }

        if (isActive.HasValue)
        {
            query = query.Where(user => (user.IsActive ?? false) == isActive.Value);
        }

        return query.OrderBy(user => user.Name).ThenBy(user => user.UserName);
    }
}
