namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

public sealed partial class CreateContractEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}