using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class DeleteClientOperation(IRepository<ClientEntity> clients)
    : IOperationHandler<DeleteClientOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteClientOperation.Request request, CancellationToken ct = default)
    {
        if (request.Id == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "Id is not correct");
        }

        return await clients.UpdateAsync(new ClientByIdSpec(request.Id), new SoftDeleteClientUpdateSpec(), ct) > 0
            ? new Success()
            : Errors.DeleteFaild;
    }
}
