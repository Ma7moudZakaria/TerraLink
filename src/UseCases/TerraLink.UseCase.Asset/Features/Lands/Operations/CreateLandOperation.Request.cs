using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Endpoints;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class CreateLandOperation
{
    public sealed record Request(CreateLandEndpoint.Request Payload) : IOperationRequest<Success>;
}
