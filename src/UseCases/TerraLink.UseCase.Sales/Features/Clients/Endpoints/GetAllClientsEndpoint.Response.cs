namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class GetAllClientsEndpoint
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public required string Name { get; set; }
            public required string Phone { get; set; }
            public required string Email { get; set; }
            public required string Code { get; set; }
            public required string Address { get; set; }
            public required string NationalId { get; set; }
        }
    }
}
