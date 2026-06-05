using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Lands.Specifications;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class GetLandDetailsOperation(IRepository<LandEntity> lands, IMapper mapper)
    : IOperationHandler<GetLandDetailsOperation.Request, GetLandDetailsOperation.Response>
{
    public async Task<ErrorOr<GetLandDetailsOperation.Response>> HandleAsync(GetLandDetailsOperation.Request request, CancellationToken ct = default)
    {
        LandEntity? land = await lands.GetAsync(new LandDetailsSpec(request.Id), ct);

        if (land is null)
        {
            return Error.NotFound("Land.NotFound", "Land was not found.");
        }

        return mapper.Map<LandEntity, GetLandDetailsOperation.Response>(land);
    }
}
