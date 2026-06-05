using ErrorOr;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.UseCase.IdentityShield.Features.Roles.Operations;

namespace TerraLink.UseCase.IdentityShield.Features.Roles.Endpoints
{
    public sealed partial class CreateRoleEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/", Handle)
                .Accepts<Request>("application/json")
                .Produces<Response>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("CreateRole")
                .WithSummary("Create Role")
                .WithDescription("Create a new role with assigned permissions")
                .AddLogging();
        }

        public static async Task<IResult> Handle([FromBody] Request request, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var result = await dispatcher.ExecuteAsync(new CreateRoleOperation.Request(request), cancellationToken);

            if (result.IsError)
            {
                return result.FirstError.ToUnprocessableEntity();
            }

                        Response response = ObjectMapper.Map<CreateRoleOperation.Response, Response>(result.Value);

            return TypedResults.Ok(response);
        }
    }
}
