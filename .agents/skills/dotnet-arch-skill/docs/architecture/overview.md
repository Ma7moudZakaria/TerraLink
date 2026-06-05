# Architecture Overview

Company .NET projects use a strict .NET 10 Clean Architecture + Vertical Slice blueprint with LowCodeHub packages as the default application framework.

## Dependency Direction

```text
API → UseCase.* → Domain
API → Integrations.* → Domain
Migration → Domain (optional)
```

Domain is the center. Dependencies always point inward.

## Project Roles

- **API** — composition host: auth wiring, middleware, OpenAPI, health checks, DI root, and module mapping.
- **UseCase.\*** — vertical slices: modules, endpoints, operations, validators, specifications, repositories/services when needed.
- **Domain** — entities, value objects, enums, DbContext, EF configurations, cross-cutting abstractions.
- **Migration** — raw SQL migration scripts scanned by LowCodeHub migration packages.
- **Integrations.\*** — provider SDK/HTTP adapters behind abstractions.
- **Tests** — unit and integration coverage.

## Core Rule

Business logic belongs in Domain entities/value objects or UseCase operations. Not in API host wiring, endpoint route mapping, EF configurations, repositories, or Integration adapters.

## Default Stack

- `LowCodeHub.MinimalEndpoints` for `IModule`, `IMinimalEndpoint`, `IOperation`, validators, and ProblemDetails helpers.
- `LowCodeHub.ObjectMapper` for trivial request/entity/DTO mapping.
- `LowCodeHub.QueryableExtensions` for specifications, generic repositories, pagination, filtering, and soft-delete.
- `LowCodeHub.OpenApi`, `LowCodeHub.Logging`, and provider-specific `LowCodeHub.Migration.*`.
- `ErrorOr` for expected operation failures.

## Existing Projects

If a project uses different folder names, map the same responsibilities to the existing folders instead of renaming everything.

## More Detail

- Folder layout → `solution-layout.md`
- Layer rules → `layers.md`
- Feature modules → `module-structure.md`
- API rules → `api-design.md`
- Database rules → `database.md`
- Integration rules → `integration.md`
- Observability → `observability.md`
- Testing → `testing.md`
