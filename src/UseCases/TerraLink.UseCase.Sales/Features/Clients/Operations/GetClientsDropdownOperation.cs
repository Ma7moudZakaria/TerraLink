using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class GetClientsDropdownOperation(IRepository<ClientEntity> clients)
    : IOperationHandler<GetClientsDropdownOperation.Request, List<GetClientsDropdownOperation.Response>>
{
    public async Task<ErrorOr<List<GetClientsDropdownOperation.Response>>> HandleAsync(GetClientsDropdownOperation.Request request, CancellationToken ct = default)
    {
        IReadOnlyList<ClientEntity> result = await clients.ListAsync(new ActiveClientsSpec(request.ClientId), ct);

        return result
            .Select(client => new GetClientsDropdownOperation.Response(client.Id, client.Name, client.NationalId))
            .ToList();
    }
}
