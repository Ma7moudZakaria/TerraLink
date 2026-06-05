using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints
{
    public sealed partial class GetCurrentUserEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/me", Handle)
          .Produces<Response>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithName("GetCurrentUser")
           .WithSummary("Get Current User")
         .WithDescription("Get currently authenticated user information")
         .RequireAuthorization()
         .AddLogging();
        }

        public static async Task<IResult> Handle(IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new GetCurrentUserOperation.Request(), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<GetCurrentUserOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
