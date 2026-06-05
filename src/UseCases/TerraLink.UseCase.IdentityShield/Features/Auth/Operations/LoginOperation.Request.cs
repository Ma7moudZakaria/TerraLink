using ErrorOr;
using LowCodeHub.MinimalEndpoints.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TerraLink.Domain.Entities;
using TerraLink.UseCase.IdentityShield.Abstracts.Shield;
using TerraLink.UseCase.IdentityShield.Features.Auth.Endpoints;

namespace TerraLink.UseCase.IdentityShield.Features.Auth.Operations;

public sealed partial class LoginOperation
{
    public sealed record Request(LoginEndpoint.Request Payload) : IOperationRequest<Response>;
}
