using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Contracts.Operations;

namespace TerraLink.UseCase.Sales.Features.Contracts.Mappers;

public sealed class ContractDetailsResponseMapper : IMapHandler<ContractEntity, GetContractDetailsOperation.Response>
{
    public GetContractDetailsOperation.Response Handler(ContractEntity source)
    {
        ClientEntity? activeClient = source.ClientContracts
            .Where(clientContract => !clientContract.IsDeleted)
            .Select(clientContract => clientContract.Client)
            .FirstOrDefault();

        string contractTypeName = source.PaymentMethod?.Descriptions.Get("ar") ?? string.Empty;

        return new GetContractDetailsOperation.Response
        {
            Id = source.Id,
            ContractNumber = source.ContractNumber,
            ContractTypeId = source.PaymentMethodId,
            ContractTypeName = contractTypeName,
            PaymentMethodId = source.PaymentMethodId,
            PaymentMethodName = contractTypeName,
            UnitPriceAtContract = source.UnitPriceAtContract,
            ContractDate = source.ContractDate,
            TotalPrice = source.TotalPrice,
            IsInstallmentPlan = source.IsInstallmentPlan,
            Notes = source.Notes,
            LandId = source.LandId,
            LandName = source.Land?.Name,
            BuildingId = source.BuildingId,
            BuildingName = source.Building?.Name,
            UnitId = source.UnitId,
            UnitName = source.Unit?.Name,
            UnitArea = source.Unit?.Area,
            UnitType = source.Unit?.UnitType?.Descriptions.Get("en"),
            UnitPrice = source.Unit?.Price,
            FloorNumber = source.Unit?.FloorNumber,
            ClientId = activeClient?.Id ?? source.ClientId,
            ClientName = activeClient?.Name ?? string.Empty,
            ClientEmail = activeClient?.Email ?? string.Empty,
            ClientPhone = activeClient?.Phone ?? string.Empty,
            ClientNationalId = activeClient?.NationalId ?? string.Empty,
            Installments = source.Installments
                .Where(installment => !installment.IsDeleted)
                .Select(installment => new GetContractDetailsOperation.InstallmentDetailsResponse
                {
                    Id = installment.Id,
                    Amount = installment.Amount,
                    AmountText = installment.AmountText,
                    Description = installment.Description,
                    DueDate = installment.DueDate
                })
                .ToList(),
            Attachments = source.Attachments,
            CreatedDate = source.CreatedDate,
            UserId = source.UserId,
            UserName = source.User?.Name
        };
    }
}
