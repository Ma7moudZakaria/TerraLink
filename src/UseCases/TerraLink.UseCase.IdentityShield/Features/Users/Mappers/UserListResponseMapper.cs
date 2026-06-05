using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Users.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Mappers;

public sealed class UserListResponseMapper : IMapHandler<UserEntity, GetUsersOperation.Response>
{
    public GetUsersOperation.Response Handler(UserEntity source)
    {
        return new GetUsersOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            UserName = source.UserName ?? string.Empty,
            Email = source.Email,
            Phone = source.Phone ?? source.PhoneNumber,
            IsActive = source.IsActive ?? false,
            RoleNames = source.UserRoles
                .Where(userRole => userRole.Role.Name != null)
                .Select(userRole => userRole.Role.Name!)
                .ToList()
        };
    }
}
