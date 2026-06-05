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
using TerraLink.UseCase.Sales.Features.Contracts.Specifications;
using static TerraLink.Domain.Constants.Constant;
using TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class UpdateContractOperation(
    IRepository<ContractEntity> contracts,
    IRepository<ClientEntity> clients,
    IRepository<ClientContractEntity> clientContracts,
    IRepository<ContractInstallmentEntity> installments,
    ITransactionManager<TerraLinkDbContext> transactionManager,
    ICurrentUserService currentUserService)
    : IOperationHandler<UpdateContractOperation.Request, Success>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateContractOperation.Request request, CancellationToken ct = default)
    {
        if (request.Id == Guid.Empty)
        {
            return Error.Validation(ErrorCode.NotValidId, "Id is not correct");
        }

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

        ContractEntity? contract = await contracts.GetAsync(new ContractByIdSpec(request.Id), ct);
        if (contract is null)
        {
            return Error.Validation(ErrorCode.NoItemExist, "There is no Contract exist");
        }

        DateTime now = DateTime.UtcNow;
        string userName = currentUserService.UserName ?? "SYSTEM";

        await transactionManager.ExecuteInTransactionAsync(async _ =>
        {
            await contracts.UpdateAsync(
                new ContractByIdSpec(request.Id),
                new UpdateContractFieldsSpec(request.Payload, client.Id, currentUserService.UserId.Value, now, userName),
                ct);

            if (contract.ClientId != client.Id)
            {
                await clientContracts.UpdateAsync(
                    new ActiveClientContractsSpec(request.Id),
                    new SoftDeleteClientContractSpec(now, userName),
                    ct);

                clientContracts.Add(new AddClientContractSpec(request.Id, client.Id, now, userName));
                await clientContracts.SaveChangesAsync(ct);
            }

            IReadOnlyList<ContractInstallmentEntity> existingInstallments =
                await installments.ListAsync(new ActiveContractInstallmentsSpec(request.Id), ct);

            HashSet<Guid> existingInstallmentIds = existingInstallments.Select(installment => installment.Id).ToHashSet();
            HashSet<Guid> incomingIds = request.Payload.Installments?
                .Where(installment => installment.Id.HasValue)
                .Select(installment => installment.Id!.Value)
                .ToHashSet() ?? [];

            List<Guid> toDelete = existingInstallmentIds.Where(existingId => !incomingIds.Contains(existingId)).ToList();
            if (toDelete.Count > 0)
            {
                await installments.UpdateAsync(
                    new ActiveContractInstallmentsByIdsSpec(request.Id, toDelete),
                    new SoftDeleteInstallmentSpec(now, userName),
                    ct);
            }

            bool hasNewInstallments = false;
            if (request.Payload.Installments is not null)
            {
                foreach (UpdateContractEndpoint.InstallmentItemRequest item in request.Payload.Installments)
                {
                    if (item.Id.HasValue && existingInstallmentIds.Contains(item.Id.Value))
                    {
                        await installments.UpdateAsync(
                            new ActiveInstallmentByIdSpec(item.Id.Value),
                            new UpdateInstallmentSpec(item, now, userName),
                            ct);
                    }
                    else
                    {
                        installments.Add(new AddInstallmentSpec(request.Id, item, now, userName));
                        hasNewInstallments = true;
                    }
                }
            }

            if (hasNewInstallments)
            {
                await installments.SaveChangesAsync(ct);
            }
        }, ct);

        return new Success();
    }
}
