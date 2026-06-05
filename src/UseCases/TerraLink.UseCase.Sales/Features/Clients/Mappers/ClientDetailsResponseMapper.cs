using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;
using TerraLink.UseCase.Sales.Features.Clients.Operations;

namespace TerraLink.UseCase.Sales.Features.Clients.Mappers;

public sealed class ClientDetailsResponseMapper : IMapHandler<ClientEntity, GetClientDetailsOperation.Response>
{
    public GetClientDetailsOperation.Response Handler(ClientEntity source)
    {
        return new GetClientDetailsOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            Phone = source.Phone,
            Email = source.Email,
            Code = source.Code,
            Address = source.Address,
            NationalId = source.NationalId,
            OwnedUnits = source.ClientContracts
                .Where(clientContract => !clientContract.IsDeleted
                    && !clientContract.Contract.IsDeleted
                    && clientContract.Contract.UnitId != null)
                .Select(ToOwnedUnitResponse)
                .ToList()
        };
    }

    private static GetClientDetailsOperation.GetClientOwnedUnitResponse ToOwnedUnitResponse(ClientContractEntity clientContract)
    {
        ContractEntity contract = clientContract.Contract;

        return new GetClientDetailsOperation.GetClientOwnedUnitResponse
        {
            ContractId = contract.Id,
            ContractNumber = contract.ContractNumber,
            ContractDate = contract.ContractDate,
            UnitId = contract.UnitId,
            UnitNumber = contract.Unit?.Number,
            UnitName = contract.Unit?.Name,
            BuildingName = contract.Building?.Name,
            LandName = contract.Land?.Name,
            FloorNumber = contract.Unit?.FloorNumber,
            NumberOfBathrooms = contract.Unit?.NumberOfBathrooms,
            NumberOfRooms = contract.Unit?.NumberOfRooms,
            UnitArea = contract.Unit?.Area,
            UnitPriceAtContract = contract.UnitPriceAtContract,
            TotalPrice = contract.TotalPrice,
            IsInstallmentPlan = contract.IsInstallmentPlan,
            Installments = contract.Installments
                .Where(installment => !installment.IsDeleted)
                .OrderBy(installment => installment.DueDate)
                .Select(ToInstallmentResponse)
                .ToList()
        };
    }

    private static GetClientDetailsOperation.GetClientInstallmentDetailsResponse ToInstallmentResponse(ContractInstallmentEntity installment)
    {
        decimal paidAmount = installment.IncomingPayments
            .Where(payment => !payment.IsDeleted && payment.SourceType == IncomingPaymentSourceType.Installment)
            .Sum(payment => payment.Amount);

        return new GetClientDetailsOperation.GetClientInstallmentDetailsResponse
        {
            InstallmentId = installment.Id,
            Description = installment.Description,
            DueDate = installment.DueDate,
            Amount = installment.Amount,
            AmountText = installment.AmountText,
            PaidAmount = paidAmount,
            IsPaid = paidAmount >= installment.Amount,
            RemainingAmount = paidAmount >= installment.Amount ? 0 : installment.Amount - paidAmount
        };
    }
}
