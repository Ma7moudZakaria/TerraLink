using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints;
using TerraLink.UseCase.IdentityShield.Features.Roles.Specifications;
using TerraLink.UseCase.IdentityShield.Features.Users.Specifications;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

public sealed partial class CreateRoleOperation
{
    public sealed record Request(CreateRoleEndpoint.Request Payload) : IOperationRequest<Response>;
}
