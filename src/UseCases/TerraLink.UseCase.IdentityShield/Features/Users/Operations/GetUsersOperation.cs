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

public sealed partial class GetUsersOperation(IRepository<UserEntity> users, IMapper mapper)
    : IOperationHandler<GetUsersOperation.Request, PagedList<GetUsersOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetUsersOperation.Response>>> HandleAsync(GetUsersOperation.Request request, CancellationToken ct = default)
    {
        PagedList<UserEntity> result = await users.PagedAsync(
            new UsersListSpec(
                request.Payload.SearchTerm,
                request.Payload.UserName,
                request.Payload.Name,
                request.Payload.Email,
                request.Payload.Phone,
                request.Payload.RoleId,
                request.Payload.IsActive),
            request.Payload.PageNumber,
            request.Payload.PageSize,
            ct);

        return new PagedList<GetUsersOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<UserEntity, GetUsersOperation.Response>).ToList()
        };
    }
}
