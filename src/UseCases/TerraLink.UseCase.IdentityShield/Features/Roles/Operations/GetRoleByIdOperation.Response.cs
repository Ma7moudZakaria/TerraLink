namespace TerraLink.UseCase.IdentityShield.Features.Roles.Operations
{
    public sealed partial class GetRoleByIdOperation
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
