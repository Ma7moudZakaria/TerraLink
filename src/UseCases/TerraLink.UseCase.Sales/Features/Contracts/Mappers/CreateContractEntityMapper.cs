using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Contracts.Mappers;

public sealed class CreateContractEntityMapper : IMapHandler<CreateContractEntityMapper.Source, ContractEntity>
{
    public sealed record Source(
        CreateContractEndpoint.Request Payload,
        Guid ClientId,
        Guid UserId,
        string ContractNumber,
        string CreatedBy);

    public ContractEntity Handler(Source source)
    {
        DateTime now = DateTime.UtcNow;
        Guid contractId = Guid.CreateVersion7();

        return new ContractEntity
        {
            Id = contractId,
            CreatedBy = source.CreatedBy,
            CreatedDate = now,
            UserId = source.UserId,
            ContractDate = source.Payload.ContractDate,
            ContractNumber = source.ContractNumber,
            PaymentMethodId = source.Payload.ContractTypeId,
            TotalPrice = source.Payload.TotalPrice,
            Notes = source.Payload.Notes,
            LandId = source.Payload.LandId,
            BuildingId = source.Payload.BuildingId,
            UnitId = source.Payload.UnitId,
            IsInstallmentPlan = source.Payload.IsInstallmentPlan,
            UnitPriceAtContract = source.Payload.UnitPriceAtContract,
            Attachments = source.Payload.Attachments,
            ClientId = source.ClientId,
            ClientContracts =
            [
                new ClientContractEntity
                {
                    Id = Guid.CreateVersion7(),
                    ContractId = contractId,
                    ClientId = source.ClientId,
                    CreatedBy = source.CreatedBy,
                    CreatedDate = now,
                    ModifiedBy = null
                }
            ],
            Installments = source.Payload.Installments?
                .Select(item => new ContractInstallmentEntity
                {
                    Id = Guid.CreateVersion7(),
                    Amount = item.Amount,
                    AmountText = item.AmountText,
                    Description = item.Description,
                    ContractId = contractId,
                    DueDate = item.DueDate,
                    CreatedBy = source.CreatedBy,
                    CreatedDate = now
                })
                .ToList() ?? []
        };
    }
}
