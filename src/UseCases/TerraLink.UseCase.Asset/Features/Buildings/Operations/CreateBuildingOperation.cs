using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.UseCase.Asset.Features.Buildings.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class CreateBuildingOperation(IRepository<BuildingEntity> buildings, ICodeGeneratorService codeGenerator)
    : IOperationHandler<CreateBuildingOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateBuildingOperation.Request request, CancellationToken ct = default)
    {
        string buildingNumber = await codeGenerator.GenerateCodeAsync<BuildingEntity>(ct);

        buildings.Add(new CreateBuildingAddSpec(request.Payload, buildingNumber));

        return await buildings.SaveChangesAsync(ct) > 0
            ? new Success()
            : Errors.CreateFaild;
    }
}
