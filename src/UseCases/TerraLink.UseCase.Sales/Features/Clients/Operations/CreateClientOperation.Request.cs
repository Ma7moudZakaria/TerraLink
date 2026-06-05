using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Endpoints;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class CreateClientOperation
{
    public sealed record Request(CreateClientEndpoint.Request Payload) : IOperationRequest<Success>;
}
