namespace TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints
{
    public sealed partial class LoginEndpoint
    {
        public sealed class Request
        {
            public string UserName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }
    }
}
