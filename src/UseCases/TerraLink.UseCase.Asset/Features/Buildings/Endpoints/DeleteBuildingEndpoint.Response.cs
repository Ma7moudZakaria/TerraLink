namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

public sealed partial class DeleteBuildingEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}