using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Contracts.Operations;

namespace TerraLink.UseCase.Sales.Features.Contracts.Mappers;

public sealed class ContractListResponseMapper : IMapHandler<ContractEntity, GetContractsOperation.Response>
{
    public GetContractsOperation.Response Handler(ContractEntity source)
    {
        return new GetContractsOperation.Response
        {
            Id = source.Id,
            ContractNumber = source.ContractNumber,
            UnitPriceAtContract = source.UnitPriceAtContract,
            ContractDate = source.ContractDate,
            Notes = source.Notes,
            IsInstallmentPlan = source.IsInstallmentPlan,
            ClientName = source.ClientContracts.FirstOrDefault(clientContract => !clientContract.IsDeleted)?.Client.Name,
            LandName = source.Land?.Name,
            BuildingName = source.Building?.Name,
            UnitName = source.Unit?.Name,
            ContractType = source.PaymentMethod?.Descriptions.Get("en"),
            TotalPrice = source.TotalPrice,
            CreatedDate = source.CreatedDate
        };
    }
}
