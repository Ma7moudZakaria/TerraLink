namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints;

public sealed partial class GetContractsDropdownEndpoint
{
    public sealed record Response(Guid Id, string Name);
}
