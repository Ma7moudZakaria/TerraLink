namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints;

public sealed partial class GetClientsDropdownEndpoint
{
    public sealed record Response(Guid Id, string Name, string NationalId);
}
