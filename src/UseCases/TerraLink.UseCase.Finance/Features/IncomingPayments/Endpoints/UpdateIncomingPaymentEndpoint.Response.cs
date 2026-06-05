namespace TerraLink.UseCase.Finance.Features.IncomingPayments.Endpoints;

public sealed partial class UpdateIncomingPaymentEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}