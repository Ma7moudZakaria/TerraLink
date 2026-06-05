using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.UseCase.Asset.Features.Buildings.Endpoints;
using TerraLink.UseCase.Asset.Features.Buildings.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class UpdateBuildingOperation
{
    public sealed record Request(Guid Id, UpdateBuildingEndpoint.Request Payload) : IOperationRequest<Success>;
}
