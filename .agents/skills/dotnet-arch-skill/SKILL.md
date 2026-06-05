---
name: dotnet-arch-skill
description: >
  Primary architecture skill for company .NET projects. Defines the strict
  .NET 10 Clean Architecture + Vertical Slice + LowCodeHub conventions:
  UseCase assemblies, MinimalEndpoints, IOperation, ErrorOr, value-object-rich
  Domain entities, EF Core complex-property mapping, LowCodeHub generic
  repositories/specifications, raw SQL migrations, observability, testing
  boundaries, and coding standards. Use this skill on every coding task in any
  company .NET project. Security is intentionally excluded — use DeepSec for
  security concerns. ADR work is delegated to adr-skill.
---

# .NET Architecture Skill

> **Status:** Primary architecture skill for all company .NET projects.
> Read the routing table below before any coding task.

## What This Skill Covers

- Layer responsibilities (API host, UseCase assemblies, Domain, Migration, Integrations)
- Solution and folder layout
- Vertical-slice feature module structure
- Minimal API design rules with `LowCodeHub.MinimalEndpoints`
- Data access patterns (EF Core + `LowCodeHub.QueryableExtensions`)
- External integration patterns
- Observability defaults
- Testing boundaries
- Coding standards (API, backend, .NET)

**Not covered here:** authentication, authorization, secrets, PII, logging of sensitive data — use DeepSec for all security concerns.

---

## Read This First

The default dependency direction:

```text
API → UseCase.* → Domain
API → Integrations.* → Domain
Migration → Domain (optional)
```

Domain is the center. Dependencies always point inward. Business logic belongs in Domain entities/value objects or UseCase operations — never in API host wiring, never in endpoint route mapping, never in Integration adapters.

---

## When to Read Which Doc

| Task | Read |
|------|------|
| Starting a new project | `docs/architecture/solution-layout.md` |
| Unsure where code belongs | `docs/architecture/layers.md` |
| Adding a new feature or module | `docs/architecture/module-structure.md` |
| Adding or changing an API endpoint | `docs/architecture/api-design.md` + `docs/standards/api.md` |
| Adding database access or migrations | `docs/architecture/database.md` |
| Adding an external provider or integration | `docs/architecture/integration.md` |
| Adding logging, metrics, or traces | `docs/architecture/observability.md` |
| Adding or reviewing tests | `docs/architecture/testing.md` |
| General .NET code review | `docs/standards/dotnet.md` + `docs/standards/backend.md` |
| Any ADR work | Use the `adr-skill` instead |
| Adding, updating, recommending, or reviewing third-party packages/dependencies | Use the `free-package-licenses` skill first |
| Anything touching mapping, endpoints/CQRS, idempotency, inbox/outbox, webhooks, RabbitMQ, SSE, durable jobs, OTP/MFA, Keycloak auth/admin, permissions, migrations, queryables, logging, OpenAPI | Use the `dotnet-lowcodehub` skill (per-package skills under `packages/`) |

---

## Key Rules (Always Apply)

### Layers
- Domain must not depend on API, UseCase, Integration projects, HTTP clients, or external provider DTOs.
- UseCase projects depend on Domain and approved LowCodeHub packages. UseCases do not reference each other.
- API is the composition host: DI, auth, middleware, OpenAPI, health checks, and `MapModules<TScanner>()`. No business logic and no feature endpoints authored directly in the host.
- Integrations adapt provider SDKs/HTTP APIs behind abstractions. Provider DTOs never cross into Domain.
- Migration contains raw SQL scripts and scanner markers. EF Core migrations are not the default.

### Modules
- UseCase assemblies own vertical slices: modules, endpoints, operation requests/handlers/responses, validators, specifications, and feature services.
- Features do not reach into each other's internals. Cross-UseCase coordination goes through events or Domain abstractions.
- Endpoint files are read in this order: Endpoints → Operations → Specifications.

### API
- Use `LowCodeHub.MinimalEndpoints`: `IModule`, `IMinimalEndpoint`, `IOperation`, `IOperationHandler<TReq,TRes>`, `IOperationRequest<TRes>`, and `IMinimalValidator<T>`.
- No MediatR, Carter, FastEndpoints, FluentValidation, or raw endpoint sprawl unless an ADR explicitly approves the exception.
- Validate input at the endpoint boundary with `IMinimalValidator<T>`.
- For endpoint companion files (`CreateFooEndpoint.Request.cs`, `.Response.cs`, `.Validator.cs`), reopen the same `public sealed partial class CreateFooEndpoint` and nest endpoint-only contracts/validator inside it.
- Authorization is enforced at the route group/endpoint boundary; business ownership is re-checked inside operations.
- Endpoints are thin: map request → dispatch operation → return typed success or `result.ToProblem()`.
- Use `ProblemResultsExtensions` from `LowCodeHub.MinimalEndpoints.Extensions`: `Error.ToProblem()`, `ErrorOr<T>.ToProblem()`, or explicit helpers such as `.ToNotFound()`. Do not use legacy `ToResponse()`.
- Do not use `EndpointResult` or `EndpointResult<T>` in new code.

### Data Access
- Entities, value objects, DbContext, and `IEntityTypeConfiguration<T>` live in Domain/Persistence for this blueprint.
- Model business scalars as value objects wherever practical: email, file extension, phone, slug, code, currency, normalized name, URL, etc.
- Value objects use `public readonly record struct`, expose `Value`, hide constructors, and validate through `static ErrorOr<TValueObject> Create(...)`.
- EF maps value objects with `ComplexProperty(...).Property(x => x.Value).HasColumnName(...).IsRequired()` or equivalent explicit conversion when complex properties are not suitable.
- Prefer `LowCodeHub.QueryableExtensions.Abstractions.IRepository<TEntity>` registered via `AddQueryableRepositories<TContext>()` over bespoke repositories.
- Queries and mutations go through `ISpecification<TEntity>`, `IAddSpecification<TEntity>`, `IAddRangeSpecification<TEntity>`, and `IUpdateSpecification<TEntity>`.
- Do not expose `IQueryable` or `DbContext` outside the repository/persistence boundary.
- Prefer no-tracking reads unless tracking is required and justified.

### Integrations
- Domain or UseCase defines the abstraction (`IEmailSender`, `IPaymentProvider`) depending on how broadly it is shared.
- Integration projects implement provider adapters.
- Domain knows nothing about HTTP clients, SDKs, or provider DTOs.

### Observability
- Do not log secrets, tokens, passwords, connection strings, or raw customer data.
- Include correlation IDs on all requests.
- Wrap external calls and heavy DB workflows in traces.

### Testing
- Unit tests for domain rules, value objects, operation handlers, validators, and specifications.
- Integration tests for API behavior, persistence mappings, migrations, and provider boundaries.
- Regression test for every bug fix.

### Code Quality
- Names describe business behavior, not implementation mechanics.
- Pass `CancellationToken` through async boundaries.
- Keep methods small and focused. No hidden side effects.
- Add dependencies only when they remove real complexity or enable required behavior.

### Company Packages — LowCodeHub
- **Package license gate:** before adding or recommending any third-party dependency, use `free-package-licenses`. Only approved permissive/free licenses may be used automatically.
- **Prefer LowCodeHub.* packages over community equivalents** for any capability they cover (mapping, endpoints/CQRS, idempotency, inbox/outbox, webhooks, RabbitMQ, SSE, durable jobs, OTP/MFA, Keycloak auth/admin, permissions, migrations, queryables, logging, OpenAPI). Reaching for AutoMapper, MediatR, MassTransit, Hangfire, Carter, Sieve, Swashbuckle, EF migrations, hand-rolled outbox, custom OTP storage, etc. requires an ADR (use `adr-skill`) explaining why the LowCodeHub equivalent is insufficient.
- **Source of truth:** https://www.nuget.org/profiles/A.Abuelnour — every `LowCodeHub.*` reference must come from this author. Reject look-alikes from other publishers.
- **Layer placement** of each LowCodeHub package is fixed — see `dotnet-lowcodehub/references/layer-allowlist.md` before adding a `PackageReference`.
- **Repository default:** use `LowCodeHub.QueryableExtensions` generic `IRepository<TEntity>` and specifications first. Custom repositories require a real project-specific reason and must still be specification-driven.
- **Endpoint response default:** use `TypedResults`/`Results` for success and `ProblemResultsExtensions` for failures.

---

## Docs Reference

```
docs/
  architecture/
    overview.md        — architecture summary and dependency map
    solution-layout.md — default folder and project structure
    layers.md          — what belongs in each layer
    module-structure.md — feature folder conventions
    api-design.md      — endpoint and response rules
    database.md        — EF Core and data access rules
    integration.md     — external provider patterns
    observability.md   — logging, metrics, traces
    testing.md         — test split and defaults
  standards/
    api.md             — API coding standards
    backend.md         — UseCase and Domain coding standards
    dotnet.md          — .NET language and framework defaults
```

---

## Agent Workflow

Before any coding task:

1. Identify the layer(s) the change touches.
2. Read the matching doc from the table above.
3. Check for any related ADR using the `adr-skill`.
4. Implement following the rules in this skill.
5. After implementation, list files touched, tests added, and any open risks.
