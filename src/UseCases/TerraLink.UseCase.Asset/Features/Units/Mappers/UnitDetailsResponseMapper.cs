using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Asset.Features.Units.Operations;

namespace TerraLink.UseCase.Asset.Features.Units.Mappers;

public sealed class UnitDetailsResponseMapper : IMapHandler<UnitEntity, GetUnitDetailsOperation.Response>
{
    public GetUnitDetailsOperation.Response Handler(UnitEntity source)
    {
        GetUnitDetailsOperation.UnitPurchaseDetailsResponse? purchaseDetails = ToPurchaseDetails(source);

        return new GetUnitDetailsOperation.Response
        {
            Id = source.Id,
            Description = source.Description,
            Building = source.Building.Name,
            Land = source.Building.Land.Name,
            Area = source.Area,
            FloorNumber = source.FloorNumber,
            NumberOfRooms = source.NumberOfRooms,
            NumberOfBatEmployeeooms = source.NumberOfBathrooms,
            UnitType = source.UnitType.Descriptions,
            UnitStatus = source.UnitStatus.Descriptions,
            Name = source.Name,
            Price = source.Price,
            Number = source.Number,
            HasBalcony = source.HasBalcony,
            HasGarage = source.HasGarage,
            HasCentralAC = source.HasCentralAC,
            FinishingType = source.FinishingType.Descriptions,
            BuildingId = source.BuildingId,
            LandId = source.Building.LandId,
            UnitTypeId = source.UnitTypeId,
            UnitStatusId = source.UnitStatusId,
            FinishingTypeId = source.FinishingTypeId,
            CreatedDate = source.CreatedDate,
            UpdatedDate = source.UpdatedDate,
            IsPurchased = purchaseDetails is not null,
            PurchaseDetails = purchaseDetails,
            Attachments = source.Attachments
        };
    }

    private static GetUnitDetailsOperation.UnitPurchaseDetailsResponse? ToPurchaseDetails(UnitEntity source)
    {
        ContractEntity? contract = source.Contracts
            .Where(contract => !contract.IsDeleted)
            .OrderByDescending(contract => contract.ContractDate)
            .ThenByDescending(contract => contract.CreatedDate)
            .FirstOrDefault();

        if (contract is null)
        {
            return null;
        }

        return new GetUnitDetailsOperation.UnitPurchaseDetailsResponse
        {
            ContractId = contract.Id,
            ContractNumber = contract.ContractNumber,
            ContractDate = contract.ContractDate,
            TotalPrice = contract.TotalPrice,
            UnitPriceAtContract = contract.UnitPriceAtContract,
            IsInstallmentPlan = contract.IsInstallmentPlan,
            Notes = contract.Notes,
            CreatedDate = contract.CreatedDate,
            UpdatedDate = contract.UpdatedDate,
            Client = contract.Client is null
                ? null
                : new GetUnitDetailsOperation.UnitPurchasedClientResponse
                {
                    Id = contract.ClientId,
                    Name = contract.Client.Name,
                    Phone = contract.Client.Phone,
                    Email = contract.Client.Email,
                    NationalId = contract.Client.NationalId,
                    Address = contract.Client.Address
                },
            Installments = contract.Installments?
                .Where(installment => !installment.IsDeleted)
                .OrderBy(installment => installment.DueDate)
                .Select(installment => new GetUnitDetailsOperation.UnitInstallmentDetailsResponse
                {
                    Id = installment.Id,
                    Description = installment.Description,
                    DueDate = installment.DueDate,
                    Amount = installment.Amount,
                    AmountText = installment.AmountText,
                    CreatedDate = installment.CreatedDate,
                    UpdatedDate = installment.UpdatedDate
                })
                .ToList() ?? []
        };
    }
}
