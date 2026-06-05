using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Mappers;

public sealed class OutgoingPaymentDetailsResponseMapper : IMapHandler<OutgoingPaymentEntity, GetOutgoingPaymentByIdOperation.Response>
{
    public GetOutgoingPaymentByIdOperation.Response Handler(OutgoingPaymentEntity source)
    {
        return new GetOutgoingPaymentByIdOperation.Response
        {
            Id = source.Id,
            Code = source.Code,
            Amount = source.Amount,
            Beneficiary = source.Beneficiary.Descriptions,
            ExpenseType = source.ExpenseType.Descriptions,
            PaymentMethod = source.PaymentMethod.Descriptions,
            PaymentDate = source.PaymentDate,
            UnitCode = source.Unit?.Number,
            BuildingCode = source.Building?.Number,
            Notes = source.Notes,
            Attachments = source.Attachments
        };
    }
}
