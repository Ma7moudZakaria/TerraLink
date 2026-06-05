namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Endpoints
{
    public sealed partial class CreateFollowUpCallEndpoint
    {
        public sealed class Request
        {
            public DateTime CallDate { get; set; }
            public required string CallerName { get; set; }
            public string? Note { get; set; }
        }
    }
}
