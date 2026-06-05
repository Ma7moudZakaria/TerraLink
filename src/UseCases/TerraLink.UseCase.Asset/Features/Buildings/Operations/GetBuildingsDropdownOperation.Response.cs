namespace TerraLink.UseCase.Asset.Features.Buildings.Operations;

public sealed partial class GetBuildingsDropdownOperation
{
    public sealed record Response(Guid Id, string Name);
}
