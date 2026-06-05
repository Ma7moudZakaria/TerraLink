namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class GetClientsDropdownOperation
{
    public sealed record Response(Guid Id, string Name, string NationalId);
}
