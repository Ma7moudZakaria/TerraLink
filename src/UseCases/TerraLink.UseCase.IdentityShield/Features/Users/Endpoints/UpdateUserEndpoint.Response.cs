namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints;

public sealed partial class UpdateUserEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}