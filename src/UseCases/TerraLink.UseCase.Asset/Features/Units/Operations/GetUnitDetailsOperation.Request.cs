using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Endpoints;
using TerraLink.UseCase.Asset.Features.Units.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class GetUnitDetailsOperation
{
    public sealed record Request(Guid Id) : IOperationRequest<Response>;
}
