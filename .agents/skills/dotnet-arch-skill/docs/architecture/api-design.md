# API Design

Use `LowCodeHub.MinimalEndpoints` for new APIs. Controllers require an explicit project decision or ADR.

## Rules

- Keep endpoints thin.
- Validate input with `IMinimalValidator<TRequest>` before operation execution.
- Enforce authorization at the API boundary.
- Re-check ownership inside the operation when business data determines access.
- Map endpoint transport DTOs to operation requests — do not expose domain entities as public contracts.
- Return consistent error responses.
- Use `IOperation` dispatch. Do not inject repositories, DbContext, entities, or provider clients into endpoints.
- Endpoint handlers should be map → execute → return. Business `if` statements belong in operations.
- For multi-file endpoint grouping, declare the same `public sealed partial class CreateFooEndpoint` across `CreateFooEndpoint.cs`, `.Request.cs`, `.Response.cs`, and `.Validator.cs`; nest endpoint-only `Request`, `Response`, and `Validator` inside that partial type. See `module-structure.md`.

## Error Responses

For expected failures, operations return `ErrorOr<T>`. Endpoints translate failures with `ProblemResultsExtensions`:

```csharp
return result.IsError
    ? result.ToProblem()
    : TypedResults.Created($"/api/foos/{result.Value}", result.Value);
```

Available helpers include `error.ToProblem()`, `result.ToProblem()`, `error.ToBadRequest()`, `error.ToNotFound()`, `error.ToConflict()`, `error.ToUnprocessableEntity()`, `error.ToUnauthorized()`, `error.ToForbidden()`, `error.ToLocked()`, and `error.ToInternalServerError()`.

Do not expose stack traces or internal exception details to clients.

## Status Codes

| Scenario | Code |
|----------|------|
| Success (data returned) | 200 |
| Resource created | 201 |
| No content | 204 |
| Validation failure | 400 |
| Unauthenticated | 401 |
| Unauthorized | 403 |
| Not found | 404 |
| Conflict | 409 |
| Server error | 500 |

## OpenAPI

- Keep OpenAPI output accurate and up to date.
- Document authentication requirements on protected endpoints.
- Document important error responses.
- Avoid leaking internal model names in schema identifiers.
- Register one OpenAPI document per UseCase with `AddOpenApiDoc("<context>", ...)`.
- Route groups must call `.WithGroupName("<context>")`.
