namespace TerraLink.UseCase.Asset.Features.Units.Endpoints;

public sealed partial class CreateUnitEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}