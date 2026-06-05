using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints;

namespace TerraLink.UseCase.IdentityShield.Features.Roles
{
    public sealed class RolesModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            // Roles management endpoints
            RouteGroupBuilder rolesGroup = app.MapGroup("/api/roles")
                .WithGroupName("identity-shield-module")
                .WithTags("Roles Management")
                .RequireAuthorization(); // All role management endpoints require authentication

            rolesGroup.MapEndpoint<GetRoleDashboardEndpoint>()
                .MapEndpoint<GetAllRolesEndpoint>()
                .MapEndpoint<GetRoleByIdEndpoint>()
                .MapEndpoint<CreateRoleEndpoint>()
                .MapEndpoint<UpdateRoleEndpoint>()
                .MapEndpoint<DeleteRoleEndpoint>();
        }
    }
}
