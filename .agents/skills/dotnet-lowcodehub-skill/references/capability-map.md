# Capability map — "I need X → use Y, never reach for Z"

| Need | Use this LowCodeHub package | Do NOT reach for (without ADR) |
|---|---|---|
| Object mapping (DTO ↔ entity, request ↔ command) | `LowCodeHub.ObjectMapper` | AutoMapper, Mapster, hand-rolled extension methods |
| Minimal API endpoint registration / grouping | `LowCodeHub.MinimalEndpoints` (`IModule`, `IMinimalEndpoint`) | Carter, FastEndpoints, manual `MapGet/Post` sprawl |
| CQRS dispatch / mediator | `LowCodeHub.MinimalEndpoints` (`IOperation` + `IOperationHandler<,>`) | MediatR, Brighter |
| Endpoint validation | `LowCodeHub.MinimalEndpoints` (`IMinimalValidator<>` + `.AddValidator<>()`) | Raw FluentValidation pipeline behaviors, manual filters |
| Endpoint logging | `LowCodeHub.MinimalEndpoints` (`.AddLogging()`) | Serilog request middleware applied per endpoint |
| Success / failure responses | Typed success results + `ProblemResultsExtensions` (`Error.ToProblem()`, `ErrorOr<T>.ToProblem()`) | `EndpointResult<T>`, legacy `ToResponse()`, hand-built ProblemDetails, raw `Results.BadRequest()` |
| Domain event bus (in-process or Redis) | `LowCodeHub.MinimalEndpoints` event bus (`IDomainEvent` + `IDomainEventHandler<>`) | MediatR notifications, MassTransit in-memory |
| Idempotent POST / PUT / PATCH | `LowCodeHub.Idempotency` (`.RequireIdempotency()`) | Custom middleware, client-driven dedupe |
| Transactional outbox | `LowCodeHub.InboxOutbox` (Outbox side) | Hand-rolled outbox, EF Core SaveChanges interceptor + queue |
| Inbox / message dedupe at consumer | `LowCodeHub.InboxOutbox` (Inbox side) | Manual unique constraints + handler logic |
| Webhook delivery to external customers | `LowCodeHub.Webhooks` | Custom retry loop, queue + worker scaffolding |
| RabbitMQ topology + consumers + publisher | `LowCodeHub.RabbitMQ` | Raw `RabbitMQ.Client`, MassTransit, EasyNetQ, NServiceBus |
| Server-Sent Events with replay | `LowCodeHub.SSE` | SignalR (unless bidirectional needed), bare `text/event-stream` writes |
| Durable / delayed / recurring / continuation / streaming jobs | `LowCodeHub.Jobs` | Hangfire, Quartz.NET, raw `IHostedService` for queues |
| OTP / one-time password / MFA challenge flow | `LowCodeHub.OTP` | Hand-rolled OTP tables, plaintext code storage, provider-coupled SMS/email OTP packages |
| JWT bearer validation against Keycloak | `LowCodeHub.Keycloak.Authentication.JwtBearer` | Hand-rolled `AddJwtBearer(...)` with manual JWKS, IdentityServer client |
| Keycloak admin API (users / groups / roles / sessions) | `LowCodeHub.Keycloak.Client` | Raw `HttpClient`, FS.Keycloak.RestApiClient |
| Permission-based authorization | `LowCodeHub.Permissions` (`.RequirePermission("...")`) | Per-permission `[Authorize(Policy=...)]` registrations, custom handlers |
| SQL Server schema migrations | `LowCodeHub.Migration.SqlServer` | EF Core `Database.Migrate()`, FluentMigrator, raw DbUp |
| PostgreSQL schema migrations | `LowCodeHub.Migration.PostgreSql` | EF Core `Database.Migrate()`, FluentMigrator, raw DbUp |
| Pagination / sorting / filtering / sparse fieldsets / dynamic filter DSL | `LowCodeHub.QueryableExtensions` | Sieve, Gridify, Linq.Dynamic.Core, hand-rolled `Skip/Take` |
| Specification pattern / generic repository | `LowCodeHub.QueryableExtensions` (`Specification<T>`, `IRepository<T>`) | Ardalis.Specification |
| Multi-tenancy global filters | `LowCodeHub.QueryableExtensions` (`ITenantContext`, `AddTenantFilters`) | Finbuckle.MultiTenant (only after ADR) |
| Soft-delete | `LowCodeHub.QueryableExtensions` (`SoftDeletableEntity`, `ApplySoftDeleteFilter`) | Hand-rolled `IsDeleted` + global filter |
| Auditing on EF SaveChanges / ExecuteUpdate / ExecuteDelete | `LowCodeHub.QueryableExtensions` (`SaveChangesAuditedAsync`, `AuditScope`) | EFCore.Audit, custom interceptors |
| Optimistic concurrency retry | `LowCodeHub.QueryableExtensions` (`RetryOnConcurrencyConflictAsync`) | Manual catch-loop |
| Query result caching | `LowCodeHub.QueryableExtensions` (`ToCachedListAsync`, `IQueryCache`) | EFCore.Cache, manual `IMemoryCache` wrappers |
| Serilog setup, HTTP enrichment, PII-safe logging | `LowCodeHub.Logging` (`AddEnhancedSerilogLogging`) | Bare Serilog + per-app config sprawl |
| OpenAPI document with security + error responses + versioning | `LowCodeHub.OpenApi` (`AddOpenApiDoc`) | Swashbuckle, NSwag |

## Anti-patterns (encode as code-review failures)

- Adding `MediatR`, `AutoMapper`, `MassTransit`, `Hangfire`, `Carter`, `Sieve`, `Gridify`, `Swashbuckle.AspNetCore`, `Ardalis.Specification`, OTP/SMS packages with built-in persistence flow, or `dbup-*` directly to a project that already references the equivalent `LowCodeHub.*` package.
- Mixing `MediatR` and `IOperation` in the same solution.
- Hand-rolling an outbox table when `LowCodeHub.InboxOutbox` is already referenced elsewhere in the solution.
- Bypassing `IPermissionService` with raw `[Authorize(Roles = ...)]` checks for permission-style names.
- Using EF Core migrations alongside `LowCodeHub.Migration.*` — pick one strategy per database.
