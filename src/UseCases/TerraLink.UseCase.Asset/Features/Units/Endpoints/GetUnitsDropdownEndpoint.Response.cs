namespace TerraLink.UseCase.Asset.Features.Units.Endpoints;

public sealed partial class GetUnitsDropdownEndpoint
{
    public sealed record Response(Guid Id, string Name);
}
