using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.IncomingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Mappers;

public sealed class IncomingPaymentDetailsResponseMapper : IMapHandler<IncomingPaymentEntity, GetIncomingPaymentByIdOperation.Response>
{
    public GetIncomingPaymentByIdOperation.Response Handler(IncomingPaymentEntity source)
    {
        return new GetIncomingPaymentByIdOperation.Response
        {
            Id = source.Id,
            Amount = source.Amount,
            ClientName = source.Client.Name,
            Attachments = source.Attachments,
            Notes = source.Notes,
            PaymentDate = source.PaymentDate,
            PaymentMethod = source.PaymentMethod.Descriptions,
            SourceType = source.SourceType,
            TransactionType = source.TransactionType.Descriptions,
            UnitCode = source.Contract.Unit?.Number,
            ContractCode = source.Contract.ContractNumber,
            ContractInstallmentId = source.ContractInstallmentId
        };
    }
}
