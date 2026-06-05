using LowCodeHub.QueryableExtensions.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TerraLink.Domain.Entities;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;

public sealed class RoleByIdSpec(Guid roleId, bool includePermissions = false, bool includeUsers = false) : ISpecification<RoleEntity>
{
    public IQueryable<RoleEntity> Where(IQueryable<RoleEntity> query)
    {
        query = query.Where(role => role.Id == roleId);

        if (includePermissions)
        {
            query = query.Include(role => role.RolePermissions)
                         .ThenInclude(rolePermission => rolePermission.PermissionLookupItem);
        }

        if (includeUsers)
        {
            query = query.Include(role => role.UserRoles);
        }

        return query;
    }
}

public sealed class RoleByNameSpec(string name, Guid? excludeId = null) : ISpecification<RoleEntity>
{
    public IQueryable<RoleEntity> Where(IQueryable<RoleEntity> query)
    {
        query = query.Where(role => role.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(role => role.Id != excludeId.Value);
        }

        return query;
    }
}

public sealed class RolesOrderedSpec(bool includeUsers = false) : ISpecification<RoleEntity>
{
    public IQueryable<RoleEntity> Where(IQueryable<RoleEntity> query)
    {
        if (includeUsers)
        {
            query = query.Include(role => role.UserRoles);
        }

        return query.OrderBy(role => role.Name);
    }
}

public sealed class RolesByIdsSpec(IReadOnlyCollection<Guid> roleIds) : ISpecification<RoleEntity>
{
    public IQueryable<RoleEntity> Where(IQueryable<RoleEntity> query)
    {
        return query.Where(role => roleIds.Contains(role.Id));
    }
}

public sealed class AddRoleSpec(Guid roleId, string name, string? description, IReadOnlyCollection<Guid> permissionIds)
    : IAddSpecification<RoleEntity>
{
    public RoleEntity Add()
    {
        return new RoleEntity
        {
            Id = roleId,
            Name = name,
            NormalizedName = name.ToUpperInvariant(),
            Description = description,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            RolePermissions = permissionIds.Select(permissionId => new RolePermissionEntity
            {
                Id = Guid.CreateVersion7(),
                RoleId = roleId,
                PermissionLookupItemId = permissionId
            }).ToList()
        };
    }
}

public sealed class UpdateRoleSpec(string name, string? description) : IUpdateSpecification<RoleEntity>
{
    public Action<UpdateSettersBuilder<RoleEntity>> Update()
    {
        return setters => setters
            .SetProperty(role => role.Name, name)
            .SetProperty(role => role.NormalizedName, name.ToUpperInvariant())
            .SetProperty(role => role.Description, description)
            .SetProperty(role => role.ConcurrencyStamp, Guid.NewGuid().ToString());
    }
}

public sealed class RolePermissionsByRoleIdSpec(Guid roleId) : ISpecification<RolePermissionEntity>
{
    public IQueryable<RolePermissionEntity> Where(IQueryable<RolePermissionEntity> query)
    {
        return query.Where(rolePermission => rolePermission.RoleId == roleId);
    }
}

public sealed class AddRolePermissionsSpec(Guid roleId, IReadOnlyCollection<Guid> permissionIds)
    : IAddRangeSpecification<RolePermissionEntity>
{
    public IEnumerable<RolePermissionEntity> AddRange()
    {
        return permissionIds.Select(permissionId => new RolePermissionEntity
        {
            Id = Guid.CreateVersion7(),
            RoleId = roleId,
            PermissionLookupItemId = permissionId
        });
    }
}

public sealed class UserRoleByRoleIdsSpec(IReadOnlyCollection<Guid> roleIds) : ISpecification<UserRoleEntity>
{
    public IQueryable<UserRoleEntity> Where(IQueryable<UserRoleEntity> query)
    {
        return query.Where(userRole => roleIds.Contains(userRole.RoleId));
    }
}
