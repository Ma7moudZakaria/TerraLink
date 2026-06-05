namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints
{
    public sealed partial class CreateUserEndpoint
    {
        public sealed class Request
        {
            public string Name { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string Password { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
            public bool IsActive { get; set; } = true;
            public List<Guid>? RoleIds { get; set; }
        }
    }
}
