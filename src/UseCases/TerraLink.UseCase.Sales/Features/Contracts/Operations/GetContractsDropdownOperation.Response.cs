namespace TerraLink.UseCase.Sales.Features.Contracts.Operations;

public sealed partial class GetContractsDropdownOperation
{
    public sealed record Response(Guid Id, string Name);
}
