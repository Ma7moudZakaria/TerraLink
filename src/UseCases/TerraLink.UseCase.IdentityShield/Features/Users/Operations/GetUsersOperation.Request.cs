using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Endpoints;
using static TerraLink.UseCase.IdentityShield.Features.Users.Operations.UserOperationHelpers;

namespace TerraLink.UseCase.IdentityShield.Features.Users.Operations;

public sealed partial class GetUsersOperation
{
    public sealed record Request(GetUsersEndpoint.Request Payload) : IOperationRequest<PagedList<Response>>;
}
