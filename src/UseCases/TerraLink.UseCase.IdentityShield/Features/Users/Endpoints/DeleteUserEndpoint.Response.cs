namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints;

public sealed partial class DeleteUserEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}