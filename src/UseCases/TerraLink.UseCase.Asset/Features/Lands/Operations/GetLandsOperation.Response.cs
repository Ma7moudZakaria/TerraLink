using LowCodeHub.QueryableExtensions.ValueObjects;

namespace TerraLink.UseCase.Asset.Features.Lands.Operations
{
    public sealed partial class GetLandsOperation
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public required string Number { get; set; }
            public required string Name { get; set; }
            public required Localized District { get; set; }
            public required Localized City { get; set; }
            public required Localized Governorate { get; set; }
            public decimal Area { get; set; }
            public int BuildingsCount { get; set; }
            public int UnitsCount { get; set; }
            public DateTime CreationDate { get; set; }
        }
    }
}
