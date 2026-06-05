namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations
{
    public sealed partial class GetRolesOperation
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public int UserCount { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
