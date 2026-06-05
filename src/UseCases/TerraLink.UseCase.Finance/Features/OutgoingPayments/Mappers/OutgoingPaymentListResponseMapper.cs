using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Finance.Features.OutgoingPayments.Operations;

namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Mappers;

public sealed class OutgoingPaymentListResponseMapper : IMapHandler<OutgoingPaymentEntity, GetOutgoingPaymentsOperation.Response>
{
    public GetOutgoingPaymentsOperation.Response Handler(OutgoingPaymentEntity source)
    {
        return new GetOutgoingPaymentsOperation.Response
        {
            Id = source.Id,
            Code = source.Code,
            Amount = source.Amount,
            Beneficiary = source.Beneficiary.Descriptions,
            ExpenseType = source.ExpenseType.Descriptions,
            PaymentMethod = source.PaymentMethod.Descriptions,
            PaymentDate = source.PaymentDate,
            UnitCode = source.Unit?.Number,
            BuildingCode = source.Building?.Number
        };
    }
}
