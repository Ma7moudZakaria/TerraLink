using LowCodeHub.MinimalEndpoints.Abstractions;
using LowCodeHub.ObjectMapper;
using LowCodeHub.MinimalEndpoints.Extensions;
using LowCodeHub.MinimalEndpoints.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TerraLink.Domain.Authorization;
using LowCodeHub.Permissions;
using TerraLink.UseCase.Sales.Features.Contracts.Operations;

namespace TerraLink.UseCase.Sales.Features.Contracts.Endpoints
{
    public sealed partial class GetContractsDropdownEndpoint : IMinimalEndpoint
    {
        public static void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/dropdown", Handle)
             .Produces<List<Response>>(StatusCodes.Status200OK)
             .AddLogging()
             .WithName("GetContractsDropdown")
             .WithSummary("Get Contracts for Dropdown")
             .WithDescription("Returns a simplified list of contracts (Id and Name) for use in dropdown/select controls. Optionally filter by contractId.")
             .RequirePermission(Permissions.Contracts.View);
        }

        public static async Task<IResult> Handle([FromQuery] Guid? contractTypeId, IOperation dispatcher, CancellationToken cancellationToken)
        {
            var contracts = await dispatcher.ExecuteAsync(new GetContractsDropdownOperation.Request(contractTypeId), cancellationToken);

            if (contracts.IsError)
            {
                return contracts.ToProblem();
            }

            List<Response> response = ObjectMapper.MapList<GetContractsDropdownOperation.Response, Response>(contracts.Value);

            return TypedResults.Ok(response);
        }
    }
}
