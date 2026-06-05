namespace TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints;

public sealed partial class LogoutEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}