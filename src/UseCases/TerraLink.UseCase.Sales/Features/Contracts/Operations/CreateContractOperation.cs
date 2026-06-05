using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using LowCodeHub.QueryableExtensions.Models;
using LowCodeHub.QueryableExtensions.Transactions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Interfaces;
using TerraLink.Domain.Interfaces.Services;
using TerraLink.Domain.Persistence;
using TerraLink.UseCase.Sales.Features.Clients.Specifications;
using TerraLink.UseCase.Sales.Features.Contracts.Mappers;
using TerraLink.UseCase.Sales.Features.Contracts.Specifications;
using static TerraLink.Domain.Constants.Constant;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class CreateContractOperation(
    IRepository<ContractEntity> contracts,
    IRepository<ClientEntity> clients,
    ICodeGeneratorService codeGeneratorService,
    ICurrentUserService currentUserService,
    IMapper mapper)
    : IOperationHandler<CreateContractOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(CreateContractOperation.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Payload.ClientNationalId))
        {
            return Error.Validation(ErrorCode.NoItemCorrect, "Client national id is required");
        }

        ClientEntity? client = await clients.GetAsync(new ClientByNationalIdSpec(request.Payload.ClientNationalId), ct);

        if (client is null)
        {
            return Error.Validation(ErrorCode.NoItemExist, "Client is not exist");
        }

        if (!currentUserService.UserId.HasValue || currentUserService.UserId.Value == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NoAuthorized, "Current user is not available.");
        }

        string contractNumber = await codeGeneratorService.GenerateCodeAsync<ContractEntity>(ct);
        string userName = currentUserService.UserName ?? "SYSTEM";

        ContractEntity contract = mapper.Map<CreateContractEntityMapper.Source, ContractEntity>(new CreateContractEntityMapper.Source(
            request.Payload,
            client.Id,
            currentUserService.UserId.Value,
            contractNumber,
            userName));

        contracts.Add(new CreateContractAddSpec(contract));

        return await contracts.SaveChangesAsync(ct) > 0
            ? new Success()
            : Errors.CreateFaild;
    }
}
