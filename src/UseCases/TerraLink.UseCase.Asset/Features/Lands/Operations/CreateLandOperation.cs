using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class CreateLandOperation(IRepository<LandEntity> lands)
    : IOperationHandler<CreateLandOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateLandOperation.Request request, CancellationToken ct = default)
    {
        string landNumber = await GenerateNumberAsync(lands, ct);

        lands.Add(new CreateLandAddSpec(request.Payload, landNumber));

        return await lands.SaveChangesAsync(ct) > 0
            ? new Success()
            : Errors.CreateFaild;
    }

    private static async Task<string> GenerateNumberAsync(IRepository<LandEntity> lands, CancellationToken ct)
    {
        int year = DateTime.UtcNow.Year;
        int countThisYear = await lands.CountAsync(new LandsCreatedInYearSpec(year), ct);

        return $"LAND-{year}-{countThisYear + 1:D4}";
    }
}
