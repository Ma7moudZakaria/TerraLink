namespace TerraLink.UseCase.Asset.Features.Lands.Endpoints;

public sealed partial class GetLandsDropdownEndpoint
{
    public sealed record Response(Guid Id, string Name);
}
