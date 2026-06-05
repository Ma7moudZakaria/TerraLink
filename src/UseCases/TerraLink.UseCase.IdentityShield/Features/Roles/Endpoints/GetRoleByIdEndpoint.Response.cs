namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints
{
    public sealed partial class GetRoleByIdEndpoint
    {
        public sealed class Response
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public List<Guid> PermissionIds { get; set; } = [];
        }
    }
}
