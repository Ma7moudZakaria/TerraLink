using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Users.Endpoints;

namespace TerraLink.UseCase.IdentityShield.Features.Users
{
    public sealed class UsersModule : IModule
    {
        public static void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder usersGroup = app.MapGroup("/api/users")
                .WithGroupName("identity-shield-module")
                .WithTags("Users Management")
                .RequireAuthorization();

            usersGroup.MapEndpoint<GetUsersEndpoint>()
                      .MapEndpoint<GetUserByIdEndpoint>()
                      .MapEndpoint<CreateUserEndpoint>()
                      .MapEndpoint<UpdateUserEndpoint>()
                      .MapEndpoint<DeleteUserEndpoint>();
        }
    }
}
