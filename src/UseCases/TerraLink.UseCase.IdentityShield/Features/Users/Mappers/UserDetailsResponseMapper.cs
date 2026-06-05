using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Features.Users.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Mappers;

public sealed class UserDetailsResponseMapper : IMapHandler<UserEntity, GetUserByIdOperation.Response>
{
    public GetUserByIdOperation.Response Handler(UserEntity source)
    {
        return new GetUserByIdOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            UserName = source.UserName ?? string.Empty,
            Email = source.Email,
            Phone = source.Phone ?? source.PhoneNumber,
            IsActive = source.IsActive ?? false,
            RoleIds = source.UserRoles.Select(userRole => userRole.RoleId).ToList(),
            RoleNames = source.UserRoles
                .Where(userRole => userRole.Role.Name != null)
                .Select(userRole => userRole.Role.Name!)
                .ToList()
        };
    }
}
