using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.QueryableExtensions.Abstractions;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.Sales.Features.Shared.Operations;
using TerraLink.UseCase.Sales.Features.Shared.Specifications;
using TerraLink.UseCase.Sales.Features.Clients.Endpoints;

namespace TerraLink.UseCase.Sales.Features.Clients.Operations;

public sealed partial class GetClientOverviewOperation(
    IRepository<ClientEntity> clients,
    IRepository<ContractEntity> contracts,
    IRepository<ContractInstallmentEntity> installments,
    IRepository<IncomingPaymentEntity> incomingPayments)
    : IOperationHandler<GetClientOverviewOperation.Request, GetClientOverviewOperation.Response>
{
    public async Task<ErrorOr<GetClientOverviewOperation.Response>> HandleAsync(GetClientOverviewOperation.Request request, CancellationToken ct = default)
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime last30Days = utcNow.AddDays(-30);
        DateTime today = utcNow.Date;

        IReadOnlyList<ClientEntity> activeClients = await clients.ListAsync(new ActiveSalesClientsOverviewSpec(), ct);
        IReadOnlyList<ContractEntity> activeContracts = await contracts.ListAsync(new ActiveSalesContractsOverviewSpec(), ct);
        List<SalesInstallmentStatusRow> installmentRows = await SalesOverviewReadModels.GetInstallmentStatusRowsAsync(installments, incomingPayments, ct);

        Dictionary<Guid, ClientEntity> clientsById = activeClients.ToDictionary(client => client.Id);
        ILookup<Guid, ContractEntity> contractsByClient = activeContracts.ToLookup(contract => contract.ClientId);

        List<GetClientOverviewOperation.ClientOverdueResponse> overdueClients = installmentRows
            .Where(row => row.RemainingAmount > 0m && row.DueDate.Date < today)
            .GroupBy(row => new { row.ClientId, row.ClientName })
            .Select(group => new GetClientOverviewOperation.ClientOverdueResponse
            {
                ClientId = group.Key.ClientId,
                ClientName = group.Key.ClientName,
                OverdueAmount = group.Sum(row => row.RemainingAmount),
                OverdueInstallmentsCount = group.Count()
            })
            .OrderByDescending(row => row.OverdueAmount)
            .ThenByDescending(row => row.OverdueInstallmentsCount)
            .ThenBy(row => row.ClientName)
            .ToList();

        List<GetClientOverviewOperation.TopClientByContractValueResponse> topClients = activeContracts
            .Where(contract => clientsById.ContainsKey(contract.ClientId))
            .GroupBy(contract => contract.ClientId)
            .Select(group =>
            {
                ClientEntity client = clientsById[group.Key];
                return new GetClientOverviewOperation.TopClientByContractValueResponse
                {
                    ClientId = client.Id,
                    ClientName = client.Name,
                    Phone = client.Phone,
                    TotalContractValue = group.Sum(contract => contract.TotalPrice),
                    ContractsCount = group.Count()
                };
            })
            .OrderByDescending(client => client.TotalContractValue)
            .Take(5)
            .ToList();

        List<GetClientOverviewOperation.RecentClientResponse> recentClients = activeClients
            .OrderByDescending(client => client.CreatedDate)
            .Take(5)
            .Select(client =>
            {
                IEnumerable<ContractEntity> clientContracts = contractsByClient[client.Id];
                return new GetClientOverviewOperation.RecentClientResponse
                {
                    ClientId = client.Id,
                    ClientName = client.Name,
                    Phone = client.Phone,
                    CreatedDate = client.CreatedDate,
                    ContractsCount = clientContracts.Count(),
                    TotalContractValue = clientContracts.Sum(contract => contract.TotalPrice)
                };
            })
            .ToList();

        Dictionary<Guid, GetClientOverviewOperation.ClientOverdueResponse> overdueByClientId = overdueClients
            .GroupBy(client => client.ClientId)
            .ToDictionary(group => group.Key, group => group.First());

        foreach (GetClientOverviewOperation.RecentClientResponse recentClient in recentClients)
        {
            if (overdueByClientId.TryGetValue(recentClient.ClientId, out GetClientOverviewOperation.ClientOverdueResponse? overdueClient))
            {
                recentClient.OverdueAmount = overdueClient.OverdueAmount;
                recentClient.HasOverdueInstallments = overdueClient.OverdueAmount > 0m;
            }
        }

        return new GetClientOverviewOperation.Response
        {
            TotalClients = activeClients.Count,
            ActiveClients = activeContracts.Select(contract => contract.ClientId).Where(clientsById.ContainsKey).Distinct().Count(),
            ClientsWithOverdueInstallments = overdueClients.Count,
            NewClientsLast30Days = activeClients.Count(client => client.CreatedDate >= last30Days),
            TopClientsByContractValue = topClients,
            OverdueClients = overdueClients.Take(10).ToList(),
            RecentClients = recentClients
        };
    }
}
