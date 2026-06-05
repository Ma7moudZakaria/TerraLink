namespace TerraLink.UseCase.Sales.Features.FollowUpCalls.Operations
{
    public sealed partial class GetFollowUpCallsOperation
    {
        public sealed class Response
        {
            public Guid FollowCallId { get; set; }
            public DateTime CallDate { get; set; }
            public required string CallerName { get; set; }
            public string? Note { get; set; }
            public Guid ClientId { get; set; }
            public required string ClientName { get; set; }
        }
    }
}
