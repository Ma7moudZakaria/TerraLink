using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class GetClientDetailsOperation(IRepository<ClientEntity> clients, IMapper mapper)
    : IOperationHandler<GetClientDetailsOperation.Request, GetClientDetailsOperation.Response>
{
    public async Task<ErrorOr<GetClientDetailsOperation.Response>> HandleAsync(GetClientDetailsOperation.Request request, CancellationToken ct = default)
    {
        ClientEntity? client = await clients.GetAsync(new ClientDetailsSpec(request.Id), ct);

        if (client is null)
        {
            return Error.Validation(ErrorCode.NoItemExist, "There is no client exist");
        }

        return mapper.Map<ClientEntity, GetClientDetailsOperation.Response>(client);
    }
}
