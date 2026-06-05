using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class CreateClientOperation(IRepository<ClientEntity> clients)
    : IOperationHandler<CreateClientOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateClientOperation.Request request, CancellationToken ct = default)
    {
        if (await clients.CountAsync(new ClientByNationalIdSpec(request.Payload.NationalId), ct) > 0)
        {
            return Error.Validation(ErrorCode.NoItemExist, "Client is already exist");
        }

        string guidPart = Guid.CreateVersion7().ToString("N")[..6].ToUpper();
        string year = DateTime.UtcNow.Year.ToString();

        clients.Add(new CreateClientAddSpec(request.Payload, $"CL-{guidPart}-{year}"));

        return await clients.SaveChangesAsync(ct) > 0
            ? new Success()
            : Errors.CreateFaild;
    }
}
