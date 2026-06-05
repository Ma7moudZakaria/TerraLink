# Module Structure

Use feature folders inside UseCase assemblies for application behavior.

## Recommended Shape

```text
src/
  UseCases/
    <Project>.UseCase.<Context>/
      I<Context>Scanner.cs
      <Context>UseCaseExtensions.cs
      Features/
        <Feature>/
          <Feature>Module.cs
          Endpoints/
            CreateFooEndpoint.cs
            CreateFooEndpoint.Request.cs
            CreateFooEndpoint.Response.cs
            CreateFooEndpoint.Validator.cs
          Operations/
            CreateFooOperation.cs
            CreateFooOperation.Request.cs
            CreateFooOperation.Response.cs
          Specifications/
            FooByIdSpec.cs
            CreateFooAddSpec.cs
            UpdateFooUpdateSpec.cs
```

## Rules

- A feature has a clear owner and purpose.
- Keep files close to the behavior they support.
- Features do not depend directly on each other's internals.
- Share code through Domain concepts or explicit abstractions.
- Endpoint files stay thin; operation handlers own business orchestration.
- Split endpoint DTOs and validators with the `CreateFooEndpoint.*.cs` partial-class pattern shown below.
- Specifications are the only way to express repository filters, projections, adds, updates, and deletes.
- Use `LowCodeHub.ObjectMapper` for trivial endpoint request → operation request and operation request → entity mapping.

## Endpoint Partial-Class Grouping

When an endpoint uses companion files such as `CreateFooEndpoint.Request.cs`, `CreateFooEndpoint.Response.cs`, and `CreateFooEndpoint.Validator.cs`, all companion files reopen the same endpoint type:

- `CreateFooEndpoint.cs` declares `public sealed partial class CreateFooEndpoint : IMinimalEndpoint` and owns route registration plus the thin handler.
- `CreateFooEndpoint.Request.cs` declares `public sealed partial class CreateFooEndpoint` and nests `Request`.
- `CreateFooEndpoint.Response.cs` declares `public sealed partial class CreateFooEndpoint` and nests `Response`.
- `CreateFooEndpoint.Validator.cs` declares `public sealed partial class CreateFooEndpoint` and nests `Validator : IMinimalValidator<Request>`.
- Keep accessibility and modifiers identical across partial declarations: `public sealed partial class CreateFooEndpoint`.
- Put `: IMinimalEndpoint` only on the route file unless there is a clear reason to repeat the interface.
- Use the same namespace in all four files.
- Do not create sibling top-level DTOs such as `CreateFooRequest` or `CreateFooResponse` for endpoint-only contracts unless another slice must reuse them.

```csharp
// CreateFooEndpoint.cs
namespace Contoso.UseCase.Catalog.Features.Foos.Endpoints;

public sealed partial class CreateFooEndpoint : IMinimalEndpoint
{
    public static void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/foos", HandleAsync)
            .AddValidator<Request>();
    }

    private static async Task<IResult> HandleAsync(
        Request request,
        IOperation operation,
        CancellationToken cancellationToken)
    {
        var result = await operation.ExecuteAsync(request, cancellationToken);

        if (result.IsError)
        {
            return result.ToProblem();
        }

        return TypedResults.Created($"/foos/{result.Value.Id}", result.Value);
    }
}
```

```csharp
// CreateFooEndpoint.Request.cs
namespace Contoso.UseCase.Catalog.Features.Foos.Endpoints;

public sealed partial class CreateFooEndpoint
{
    public sealed record Request(string Name) : IOperationRequest<Response>;
}
```

```csharp
// CreateFooEndpoint.Response.cs
namespace Contoso.UseCase.Catalog.Features.Foos.Endpoints;

public sealed partial class CreateFooEndpoint
{
    public sealed record Response(Guid Id, string Name);
}
```

```csharp
// CreateFooEndpoint.Validator.cs
namespace Contoso.UseCase.Catalog.Features.Foos.Endpoints;

public sealed partial class CreateFooEndpoint
{
    public sealed class Validator : IMinimalValidator<Request>
    {
        public IEnumerable<ValidationFailure> Validate(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                yield return new ValidationFailure(nameof(Request.Name), "Name is required.");
            }
        }
    }
}
```

This grouping keeps endpoint-only contracts discoverable without making the feature folder depend on broad shared DTO names. The example is about the partial-class file layout, not request/response conversion.

## Naming

Use names that describe business behavior, not implementation mechanics.

Good:
- `CreateInvoice`
- `ApproveRefund`
- `GetPatientSummary`
- `SyncProviderClaim`

Avoid:
- `InvoiceService`
- `DataManager`
- `Helper`
- `Processor`

## UseCase Registration

Each UseCase exposes one `Add<Context>UseCase(this IServiceCollection services)` method. It registers validators, operations, event bus handlers when needed, `AddOpenApiDoc("<context>", ...)`, and `services.AddQueryableRepositories<TContext>()` when generic repositories are used.
