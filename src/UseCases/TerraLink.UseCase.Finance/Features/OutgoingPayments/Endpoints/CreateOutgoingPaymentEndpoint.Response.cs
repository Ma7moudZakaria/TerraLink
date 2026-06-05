namespace TerraLink.UseCase.Finance.Features.OutgoingPayments.Endpoints;

public sealed partial class CreateOutgoingPaymentEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}