namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

public sealed partial class CreateBuildingEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}