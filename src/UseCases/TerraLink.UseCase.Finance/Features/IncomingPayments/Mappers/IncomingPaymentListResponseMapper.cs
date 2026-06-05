using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Mappers;

public sealed class IncomingPaymentListResponseMapper : IMapHandler<IncomingPaymentEntity, GetIncomingPaymentsOperation.Response>
{
    public GetIncomingPaymentsOperation.Response Handler(IncomingPaymentEntity source)
    {
        return new GetIncomingPaymentsOperation.Response
        {
            Id = source.Id,
            Amount = source.Amount,
            ClientName = source.Client.Name,
            PaymentDate = source.PaymentDate,
            ContractInstallmentId = source.ContractInstallmentId,
            PaymentMethod = source.PaymentMethod.Descriptions,
            SourceType = source.SourceType,
            UnitCode = source.Contract.Unit?.Number,
            TransactionType = source.TransactionType.Descriptions,
            ContractCode = source.Contract.ContractNumber
        };
    }
}
