namespace TerraLink.UseCase.Finance.Features.Installments.Endpoints;

public sealed partial class UpdateInstallmentEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}