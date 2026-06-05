using LowCodeHub.MinimalEndpoints.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Clients.Operations;

namespace TerraLink.UseCase.Sales.Features.Clients.Mappers;

public sealed class ClientListResponseMapper : IMapHandler<ClientEntity, GetClientsOperation.Response>
{
    public GetClientsOperation.Response Handler(ClientEntity source)
    {
        return new GetClientsOperation.Response
        {
            Id = source.Id,
            Name = source.Name,
            Phone = source.Phone,
            Email = source.Email,
            Code = source.Code,
            Address = source.Address,
            NationalId = source.NationalId
        };
    }
}
