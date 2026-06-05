# Layer Responsibilities

## API

API project is the host and delivery layer.

May contain:
- composition root wiring
- auth, CORS, rate limiters, middleware, OpenAPI, health checks
- `app.MapModules<I<Context>Scanner>()` calls
- authentication and authorization wiring

Must not contain feature endpoints, DTOs, operations, repository logic, or business rules.

## UseCase.*

UseCase projects are bounded contexts that own vertical slices.

May contain:
- `IModule` route groups and `IMinimalEndpoint` endpoints
- `IOperationRequest<T>` and `IOperationHandler<TReq,TRes>`
- endpoint/operation DTOs
- `IMinimalValidator<T>` validators
- specifications
- context-local services
- context-local repositories only when the generic repository is insufficient

May depend on Domain and approved LowCodeHub packages. UseCases must not reference other UseCases or the API host.

## Domain

Domain project owns business concepts and rules.

May contain:
- entities
- value objects
- domain events
- enums
- domain services
- cross-cutting abstractions
- DbContext and EF Core configuration for this blueprint

Must not depend on API, UseCase projects, Integration projects, HTTP clients, SDKs, or external provider DTOs.

## Integrations.*

Integration projects own external provider details.

May contain:
- email, SMS, storage, queue, provider integrations
- options classes for external services
- typed clients and provider DTO mapping

May depend on Domain and the abstractions they implement. Provider DTOs stop at this boundary.

## Migration

Migration project owns raw SQL scripts and migration scanner markers. It does not own business behavior.

## Dependency Direction

```text
API → UseCase.* → Domain
API → Integrations.* → Domain
Migration → Domain (optional)
```

Domain is center. Dependencies point inward. Any dependency that flows outward, such as Domain → UseCase or UseCase → API, is a violation.
