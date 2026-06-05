# .NET Standards

Follow the existing project's style first. For new ASP.NET projects, use these defaults.

## General

- Use clear, explicit names that describe business behavior.
- Keep methods small and focused on a single responsibility.
- Prefer constructor injection.
- Use async APIs for all I/O.
- Pass `CancellationToken` through application and infrastructure boundaries.
- Keep nullable reference types enabled.

## ASP.NET

- Keep controllers and Minimal API endpoints thin.
- Validate request input before use-case execution.
- Use authorization policies instead of scattered `[Authorize(Roles = "...")]` checks.
- Return consistent HTTP errors (Problem Details).
- Keep transport DTOs separate from domain entities.
- For new Minimal APIs, use `LowCodeHub.MinimalEndpoints` and dispatch through `IOperation`.
- Use `ProblemResultsExtensions` for `ErrorOr` failures; do not use legacy `ToResponse()`.

## EF Core

- Keep `IEntityTypeConfiguration<T>` classes focused on a single entity.
- Map value objects explicitly with `ComplexProperty` when practical.
- Prefer no-tracking reads (`AsNoTracking`) unless updates require tracking.
- Use raw SQL migrations through `LowCodeHub.Migration.<Provider>` for new projects. Never `EnsureCreated` in production.
- Avoid lazy loading unless the project has deliberately adopted it with full awareness of N+1 risks.

## Repositories and Specifications

- Prefer `LowCodeHub.QueryableExtensions.Abstractions.IRepository<TEntity>`.
- Express filters, adds, bulk adds, updates, deletes, paging, and counts through specifications.
- Do not expose `IQueryable` or `DbContext` across application boundaries.

## Testing

- Unit tests for business rules (domain and application layer).
- Integration tests for API behavior and persistence.
- Regression test for every bug fix.

## Packages

- Do not add packages because a scaffold or template includes them.
- Add a package only when the project needs it and the trade-off is clear.
- Prefer packages with active maintenance and a clear release cadence.
