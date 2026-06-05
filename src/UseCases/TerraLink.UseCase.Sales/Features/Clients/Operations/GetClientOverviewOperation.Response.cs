namespace TerraLink.UseCase.Sales.Features.Clients.Operations
{
    public sealed partial class GetClientOverviewOperation
    {
        public sealed class Response
        {
            public int TotalClients { get; set; }
            public int ActiveClients { get; set; }
            public int ClientsWithOverdueInstallments { get; set; }
            public int NewClientsLast30Days { get; set; }
            public List<TopClientByContractValueResponse> TopClientsByContractValue { get; set; } = [];
            public List<ClientOverdueResponse> OverdueClients { get; set; } = [];
            public List<RecentClientResponse> RecentClients { get; set; } = [];
        }

        public sealed class TopClientByContractValueResponse
        {
            public Guid ClientId { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public decimal TotalContractValue { get; set; }
            public int ContractsCount { get; set; }
        }

        public sealed class ClientOverdueResponse
        {
            public Guid ClientId { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public decimal OverdueAmount { get; set; }
            public int OverdueInstallmentsCount { get; set; }
        }

        public sealed class RecentClientResponse
        {
            public Guid ClientId { get; set; }
            public string ClientName { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
            public int ContractsCount { get; set; }
            public decimal TotalContractValue { get; set; }
            public decimal OverdueAmount { get; set; }
            public bool HasOverdueInstallments { get; set; }
        }
    }
}
