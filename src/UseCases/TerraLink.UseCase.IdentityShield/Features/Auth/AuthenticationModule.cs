using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints;

namespace TerraLink.UseCase.IdentityShield.Features.Auth
{
    public sealed class AuthenticationModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            // Public authentication endpoints - no authorization required
            RouteGroupBuilder publicAuthGroup = app.MapGroup("/api/auth")
                .WithGroupName("identity-shield-module")
                .WithTags("Authentication")
                .AllowAnonymous();

            publicAuthGroup.MapEndpoint<LoginEndpoint>()
                .MapEndpoint<RefreshTokenEndpoint>();

            // Protected authentication endpoints - require authentication
            RouteGroupBuilder protectedAuthGroup = app.MapGroup("/api/auth")
                .WithGroupName("identity-shield-module")
                .WithTags("Authentication")
                .RequireAuthorization();

            protectedAuthGroup.MapEndpoint<LogoutEndpoint>()
                .MapEndpoint<GetCurrentUserEndpoint>();
        }
    }
}
