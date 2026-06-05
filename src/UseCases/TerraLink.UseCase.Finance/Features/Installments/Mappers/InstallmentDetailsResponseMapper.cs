using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.Installments.Operations;

namespace TerraLink.UseCase.Finance.Features.Installments.Mappers;

public sealed class InstallmentDetailsResponseMapper : IMapHandler<ContractInstallmentEntity, GetInstallmentByIdOperation.Response>
{
    public GetInstallmentByIdOperation.Response Handler(ContractInstallmentEntity source)
    {
        decimal paidAmount = source.IncomingPayments.Where(payment => !payment.IsDeleted).Sum(payment => payment.Amount);

        return new GetInstallmentByIdOperation.Response
        {
            Id = source.Id,
            ContractId = source.ContractId,
            ContractCode = source.Contract.ContractNumber,
            ClientId = source.Contract.ClientId,
            ClientName = source.Contract.Client.Name,
            UnitId = source.Contract.UnitId,
            UnitCode = source.Contract.Unit?.Number,
            UnitName = source.Contract.Unit?.Name,
            Description = source.Description,
            DueDate = source.DueDate,
            Amount = source.Amount,
            AmountText = source.AmountText,
            PaidAmount = paidAmount,
            RemainingAmount = source.Amount - paidAmount,
            IsPaid = paidAmount >= source.Amount,
            CreatedDate = source.CreatedDate
        };
    }
}
