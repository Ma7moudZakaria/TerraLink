using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;
using TerraLink.UseCase.Sales.Features.Clients.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class UpdateClientOperation(IRepository<ClientEntity> clients)
    : IOperationHandler<UpdateClientOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateClientOperation.Request request, CancellationToken ct = default)
    {
        UpdateClientFieldsSpec updateSpec = ObjectMapper.Map<UpdateClientEndpoint.Request, UpdateClientFieldsSpec>(request.Payload);

        return await clients.UpdateAsync(new ClientByIdSpec(request.Id), updateSpec, ct) > 0
            ? new Success()
            : Errors.UpdateFaild;
    }
}
