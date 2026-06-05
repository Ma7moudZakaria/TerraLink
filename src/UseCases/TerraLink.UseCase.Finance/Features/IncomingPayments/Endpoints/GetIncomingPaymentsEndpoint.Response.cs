using LowCodeHub.QueryableExtensions.ValueObjects;
using TerraLink.Domain.Enums;

namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class GetIncomingPaymentsEndpoint
{
    public sealed class Response
    {
        public Guid Id { get; set; }
        public required string ClientName { get; set; }
        public Guid? ContractInstallmentId { get; set; }
        public required Localized TransactionType { get; set; }
        public string? UnitCode { get; set; }
        public required string ContractCode { get; set; }
        public required decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public required Localized PaymentMethod { get; set; }
        public IncomingPaymentSourceType SourceType { get; set; }
    }
}
