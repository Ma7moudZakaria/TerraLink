using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class CreateUnitOperation(IRepository<UnitEntity> units)
    : IOperationHandler<CreateUnitOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateUnitOperation.Request request, CancellationToken ct = default)
    {
        string unitNumber = await GenerateNumberAsync(units, ct);

        units.Add(new CreateUnitAddSpec(request.Payload, unitNumber));

        return await units.SaveChangesAsync(ct) > 0
            ? new Success()
            : Errors.CreateFaild;
    }

    private static async Task<string> GenerateNumberAsync(IRepository<UnitEntity> units, CancellationToken ct)
    {
        int year = DateTime.UtcNow.Year;
        int countThisYear = await units.CountAsync(new UnitsCreatedInYearSpec(year), ct);

        return $"UNIT-{year}-{countThisYear + 1:D4}";
    }
}
