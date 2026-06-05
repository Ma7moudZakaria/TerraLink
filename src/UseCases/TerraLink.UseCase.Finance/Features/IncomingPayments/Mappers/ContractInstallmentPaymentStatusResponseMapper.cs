using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.Domain.Enums;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Mappers;

public sealed class ContractInstallmentPaymentStatusResponseMapper
    : IMapHandler<ContractInstallmentEntity, GetContractInstallmentsPaymentStatusOperation.Response>
{
    public GetContractInstallmentsPaymentStatusOperation.Response Handler(ContractInstallmentEntity source)
    {
        decimal paidAmount = source.IncomingPayments
            .Where(payment => !payment.IsDeleted && payment.SourceType == IncomingPaymentSourceType.Installment)
            .Sum(payment => payment.Amount);

        return new GetContractInstallmentsPaymentStatusOperation.Response
        {
            InstallmentId = source.Id,
            Description = source.Description,
            DueDate = source.DueDate,
            Amount = source.Amount,
            AmountText = source.AmountText,
            PaidAmount = paidAmount,
            IsPaid = paidAmount >= source.Amount
        };
    }
}
