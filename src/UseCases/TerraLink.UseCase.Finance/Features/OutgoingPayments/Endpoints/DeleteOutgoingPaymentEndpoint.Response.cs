namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class DeleteOutgoingPaymentEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}