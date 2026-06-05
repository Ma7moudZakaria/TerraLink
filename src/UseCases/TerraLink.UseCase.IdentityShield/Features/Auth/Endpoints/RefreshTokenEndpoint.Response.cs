using TerraLink.UseCase.IdentityShield.Models;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints
{
    public sealed partial class RefreshTokenEndpoint
    {
        public sealed class Response
        {
            public bool Succeeded { get; set; }
            public string Message { get; set; } = string.Empty;
            public Guid? UserId { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }
            public string? RoleName { get; set; }
            public Guid? RoleId { get; set; }
            public JwtResponse? Token { get; set; }
        }
    }
}
