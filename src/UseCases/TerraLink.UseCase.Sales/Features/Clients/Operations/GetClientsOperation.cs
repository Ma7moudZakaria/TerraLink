using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class GetClientsOperation(IRepository<ClientEntity> clients, IMapper mapper)
    : IOperationHandler<GetClientsOperation.Request, PagedList<GetClientsOperation.Response>>
{
    public async Task<ErrorOr<PagedList<GetClientsOperation.Response>>> HandleAsync(GetClientsOperation.Request request, CancellationToken ct = default)
    {
        PagedList<ClientEntity> result = await clients.PagedAsync(
            new ClientsListSpec(request.Payload),
            request.Payload.PageNumber,
            request.Payload.PageSize,
            ct);

        return new PagedList<GetClientsOperation.Response>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(mapper.Map<ClientEntity, GetClientsOperation.Response>).ToList()
        };
    }
}
