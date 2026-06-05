namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints;

public sealed partial class CreateClientEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}