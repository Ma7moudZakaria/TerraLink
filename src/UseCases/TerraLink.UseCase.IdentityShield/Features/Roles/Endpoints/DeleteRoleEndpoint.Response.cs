namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints;

public sealed partial class DeleteRoleEndpoint
{
    public sealed class Response
    {
        public static Response Default { get; } = new();
    }
}