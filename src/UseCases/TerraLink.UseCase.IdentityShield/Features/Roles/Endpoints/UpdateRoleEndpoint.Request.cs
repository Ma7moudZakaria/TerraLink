namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints
{
    public sealed partial class UpdateRoleEndpoint
    {
        public sealed record Request
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public List<Guid> PermissionIds { get; set; } = [];
        }
    }
}
