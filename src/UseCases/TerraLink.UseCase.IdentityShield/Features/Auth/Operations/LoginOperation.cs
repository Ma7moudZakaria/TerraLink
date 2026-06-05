using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

public sealed partial class LoginOperation(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    RoleManager<RoleEntity> roleManager,
    IJwtTokenProvider tokenProvider)
    : IOperationHandler<LoginOperation.Request, LoginOperation.Response>
{
    public async Task<ErrorOr<LoginOperation.Response>> HandleAsync(LoginOperation.Request request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Payload.UserName))
        {
            return Error.Validation("UserName", "Username is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Payload.Password))
        {
            return Error.Validation("Password", "Password is required.");
        }

        UserEntity? user = await userManager.FindByNameAsync(request.Payload.UserName);
        if (user is null)
        {
            return Error.NotFound("Login", "Invalid username or password.");
        }

        if (user.IsActive == false)
        {
            return Error.Forbidden("Login", "User account is deactivated.");
        }

        SignInResult passwordCheck = await signInManager.CheckPasswordSignInAsync(user, request.Payload.Password, lockoutOnFailure: true);
        if (!passwordCheck.Succeeded)
        {
            return passwordCheck.IsLockedOut
                ? Error.Forbidden("Login", "Account is locked out.")
                : Error.Unauthorized("Login", "Invalid username or password.");
        }

        (Guid? roleId, string? roleName) = AuthOperationHelpers.GetPrimaryRole(roleManager, user.Id);

        return new LoginOperation.Response
        {
            Succeeded = true,
            Message = "Login successful.",
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            RoleId = roleId,
            RoleName = roleName,
            Token = await tokenProvider.GetJwtAsync(user, ct)
        };
    }
}
