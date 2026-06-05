namespace TerraLink.UseCase.IdentityShield.Features.Users.Operations
{
    public sealed partial class GetUserByIdOperation
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public bool IsActive { get; set; }
            public List<Guid> RoleIds { get; set; } = [];
            public List<string> RoleNames { get; set; } = [];
        }
    }
}
