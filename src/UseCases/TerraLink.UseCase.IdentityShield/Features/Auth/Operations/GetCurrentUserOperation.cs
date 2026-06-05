using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

public sealed partial class GetCurrentUserOperation(
    UserManager<UserEntity> userManager,
    RoleManager<RoleEntity> roleManager,
    IJwtTokenProvider tokenProvider,
    IHttpContextAccessor httpContextAccessor)
    : IOperationHandler<GetCurrentUserOperation.Request, GetCurrentUserOperation.Response>
{
    public async Task<ErrorOr<GetCurrentUserOperation.Response>> HandleAsync(GetCurrentUserOperation.Request request, CancellationToken ct = default)
    {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return Error.Unauthorized("Auth", "User is not authenticated.");
        }

        System.Security.Claims.Claim? userIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirst("sub");

        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return Error.Unauthorized("Auth", "Invalid user token.");
        }

        UserEntity? user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Error.NotFound("Auth", "User not found.");
        }

        (Guid? roleId, string? roleName) = AuthOperationHelpers.GetPrimaryRole(roleManager, user.Id);

        return new GetCurrentUserOperation.Response
        {
            Succeeded = true,
            Message = "User retrieved successfully.",
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            RoleId = roleId,
            RoleName = roleName,
            Token = await tokenProvider.GetJwtAsync(user, ct)
        };
    }
}
