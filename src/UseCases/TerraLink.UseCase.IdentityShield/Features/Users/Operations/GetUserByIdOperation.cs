using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Specifications;
using static TerraLink.UseCase.IdentityShield.Features.Users.Operations.UserOperationHelpers;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Operations;

public sealed partial class GetUserByIdOperation(IRepository<UserEntity> users, IMapper mapper)
    : IOperationHandler<GetUserByIdOperation.Request, GetUserByIdOperation.Response>
{
    public async Task<ErrorOr<GetUserByIdOperation.Response>> HandleAsync(GetUserByIdOperation.Request request, CancellationToken ct = default)
    {
        UserEntity? user = await users.GetAsync(new UserByIdSpec(request.UserId), ct);
        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        return mapper.Map<UserEntity, GetUserByIdOperation.Response>(user);
    }
}
