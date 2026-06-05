namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints;

public sealed partial class CreateRoleEndpoint
{
    public sealed class Response
    {
        public Guid Id { get; set; }
    }
}