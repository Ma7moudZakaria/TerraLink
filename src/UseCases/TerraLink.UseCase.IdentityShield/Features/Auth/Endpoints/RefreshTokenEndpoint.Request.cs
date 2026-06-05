namespace TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints
{
    public sealed partial class RefreshTokenEndpoint
    {
        public sealed class Request
        {
            public string RefreshToken { get; set; } = string.Empty;
        }
    }
}
