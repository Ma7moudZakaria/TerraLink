namespace TerraLink.UseCase.Asset.Features.Buildings.Endpoints;

public sealed partial class GetBuildingsDropdownEndpoint
{
    public sealed record Response(Guid Id, string Name);
}
