using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Sales.Features.Clients.Endpoints
{
    public sealed partial class CreateClientEndpoint
    {
        public sealed class Request
        {
            public required string Name { get; set; }
            public required string Phone { get; set; }
            public required string Email { get; set; }
            public required string NationalId { get; set; }
            public required string Address { get; set; }
            public AttributesDictionary? Attachments { get; set; }
        }
    }
}
