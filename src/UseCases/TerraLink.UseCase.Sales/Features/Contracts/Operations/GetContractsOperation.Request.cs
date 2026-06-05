using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.Sales.Features.Contracts.Endpoints;
using TerraLink.UseCase.Sales.Features.Contracts.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class GetContractsOperation
{
    public sealed record Request(GetAllContractsEndpoint.Request Payload) : IOperationRequest<PagedList<Response>>;
}
