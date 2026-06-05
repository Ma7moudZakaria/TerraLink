namespace TerraLink.UseCase.IdentityShield.Features.Users.Endpoints;

public sealed partial class CreateUserEndpoint
{
    public sealed class Response
    {
        public Guid Id { get; set; }
    }
}