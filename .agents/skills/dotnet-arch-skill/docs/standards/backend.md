# Backend Standards

## UseCase Code

- Keep UseCase operations explicit and focused — one handler, one responsibility.
- Prefer feature folders for endpoints, operations, validators, specifications, and DTOs.
- Pass `CancellationToken` through all async boundaries.
- Avoid hidden side effects. Side effects belong in explicit commands.
- Keep orchestration in operations, not in endpoints and not in repositories/integrations.
- Operations return `Task<ErrorOr<TResponse>>` for expected failures. Do not throw for normal business outcomes.

## Domain Code

- Keep entities focused on business behavior, not persistence concerns.
- Use value objects aggressively for concepts with validation rules, formatting, normalization, or structural equality.
- Prefer `public readonly record struct` value objects with `Value`, private constructors, and `static ErrorOr<T> Create(...)`.
- Keep provider contracts, persistence concerns, and transport DTOs out of Domain.
- Domain methods express intent (`invoice.Approve()` not `invoice.Status = "Approved"`).

## Persistence and Integration Code

- Keep EF Core DbContext/configurations in Domain/Persistence for this blueprint.
- Keep external providers inside Integration projects.
- Use strongly-typed options classes for configuration.
- Validate required options at startup.
- Keep provider logs safe — no secrets, no raw tokens.
- Prefer `LowCodeHub.QueryableExtensions` generic `IRepository<TEntity>` and specifications. Custom repositories must be specification-driven and justified.

## Dependencies

- Add new packages only when they remove real complexity or enable required behavior.
- Do not add a package because a template includes it.
- Evaluate each dependency's maintenance status, size, and license before adding.
