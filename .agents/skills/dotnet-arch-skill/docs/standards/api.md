# API Standards

## Request Handling

- Keep endpoints and controllers thin.
- Validate input before executing application use cases.
- Do not put business rules in controllers, endpoints, filters, or middleware.
- Use request and response DTOs at the API boundary.
- Do not expose domain entities directly as public API contracts.
- For new Minimal APIs, use `IModule`, `IMinimalEndpoint`, `IMinimalValidator<T>`, and `IOperation`.
- Endpoint handlers map endpoint request DTOs to operation requests, dispatch with `IOperation`, and return only success or `result.ToProblem()`.
- When endpoint contracts are split into `CreateFooEndpoint.Request.cs`, `.Response.cs`, and `.Validator.cs`, each file must reopen `public sealed partial class CreateFooEndpoint` and nest the endpoint-only type inside it.

## Responses

- Return consistent status codes.
- Use Problem Details for error responses.
- Use `ProblemResultsExtensions` from `LowCodeHub.MinimalEndpoints.Extensions` for expected failures.
- Do not use `EndpointResult`, `EndpointResult<T>`, or `ToResponse()` in new code.
- Do not expose stack traces or internal exception details to clients.
- Use cursor-based or offset pagination for list endpoints that can grow.

## Authorization

- Enforce authentication at the API boundary.
- Use policies for reusable authorization rules.
- Re-check ownership or business authorization inside the UseCase operation when business data determines access.

## OpenAPI

- Keep OpenAPI output accurate.
- Document authentication requirements on protected endpoints.
- Document important error responses.
- Do not leak internal model or namespace names in schema identifiers.
