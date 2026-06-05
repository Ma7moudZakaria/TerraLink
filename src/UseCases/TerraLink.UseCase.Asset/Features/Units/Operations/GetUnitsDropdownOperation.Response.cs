namespace TerraLink.UseCase.Asset.Features.Units.Operations;

public sealed partial class GetUnitsDropdownOperation
{
    public sealed record Response(Guid Id, string Name);
}
