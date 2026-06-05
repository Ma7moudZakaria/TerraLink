namespace TerraLink.UseCase.IdentityShield.Features.Users.Operations;

public sealed partial class CreateUserOperation
{
    public sealed class Response
    {
        public Guid Id { get; set; }
    }
}