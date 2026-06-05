namespace TerraLink.UseCase.Asset.Features.Lands.Operations;

public sealed partial class GetLandsDropdownOperation
{
    public sealed record Response(Guid Id, string Name);
}
