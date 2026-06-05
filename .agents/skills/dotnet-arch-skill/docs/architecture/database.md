# Data Access

Use EF Core with `LowCodeHub.QueryableExtensions` by default for ASP.NET projects unless the project already has an approved different data-access pattern.

## Location

```text
src/<Project>.Domain/
  Persistence/
    <Project>DbContext.cs
    Configurations/       ← IEntityTypeConfiguration<T> per entity
  Entities/
  ValueObjects/

src/<Project>.Migration/
  IMigrationScanner.cs
  Scripts/<Provider>/...
```

## Rules

- Keep entities, value objects, DbContext, and EF configuration in Domain for this blueprint.
- Keep persistence annotations off entities unless unavoidable; prefer fluent configuration.
- Keep migrations as raw SQL scripts through `LowCodeHub.Migration.<Provider>`. Do not use EF Core migrations for new projects.
- Prefer no-tracking reads (`AsNoTracking`).
- Do not expose `IQueryable` or `DbContext` outside the persistence/repository boundary.
- Do not perform destructive schema changes without a raw SQL migration and a backfill plan.
- Preserve soft-delete and tenant filters if the project already uses them — do not disable global query filters.
- Skip custom audit infrastructure from older blueprints. If auditing is needed, use the current `LowCodeHub.QueryableExtensions` audit APIs deliberately and read that package skill first.

## Value Objects in Entities

Prefer value objects over primitive strings for business scalars. Examples: `Email`, `AttachmentExtension`, `FileName`, `PhoneNumber`, `CurrencyCode`, `Slug`, `ExternalId`, `NormalizedName`.

Use this shape unless the existing project already has a stricter local convention:

```csharp
public readonly record struct AttachmentExtension
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".doc", ".mp4", ".m4a"
    };

    public string Value { get; }

    private AttachmentExtension(string value) => Value = value;

    public static ErrorOr<AttachmentExtension> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !IsAllowed(value))
        {
            return Error.Validation("not_allowed_format");
        }

        return new AttachmentExtension(value);
    }

    public static bool IsAllowed(string extension)
    {
        return AllowedExtensions.Contains(extension.ToLower());
    }
}
```

Rules:

- Value object constructors are private.
- Public creation returns `ErrorOr<TValueObject>`.
- Expose the primitive through `Value`.
- Keep validation inside the value object, not scattered across handlers.
- Use value objects in entity properties wherever the scalar carries rules, formatting, identity, or normalization.

## EF Mapping for Value Objects

Map value objects explicitly. For simple one-value structs, prefer `ComplexProperty`:

```csharp
builder.ComplexProperty(a => a.Extension, a =>
{
    a.Property(x => x.Value)
        .HasMaxLength(50)
        .HasColumnName("Extension")
        .IsRequired();
});
```

Use explicit conversions only when complex properties are not suitable for the provider or EF version.

## Repository Pattern

Default to the internal generic repository from `LowCodeHub.QueryableExtensions`:

```csharp
using LowCodeHub.QueryableExtensions.Extensions;

services.AddQueryableRepositories<AppDbContext>();
```

Operations inject `IRepository<TEntity>` and pass specifications:

```csharp
public sealed class CreateFooOperation(IRepository<Foo> foos)
    : IOperationHandler<CreateFooOperationRequest, Guid>
{
    public async Task<ErrorOr<Guid>> HandleAsync(CreateFooOperationRequest request, CancellationToken ct = default)
    {
        var spec = new CreateFooAddSpec(request);
        foos.Add(spec);
        await foos.SaveChangesAsync(ct);
        return spec.Id;
    }
}
```

Do not create bespoke repositories by default. A custom repository is allowed only when the generic repository cannot express the workflow cleanly; it must still accept `ISpecification<TEntity>`, `IAddSpecification<TEntity>`, `IAddRangeSpecification<TEntity>`, or `IUpdateSpecification<TEntity>` rather than ad-hoc `GetByX` methods.

## Specifications

- Filters use `ISpecification<TEntity>`.
- Adds use `IAddSpecification<TEntity>`.
- Bulk adds use `IAddRangeSpecification<TEntity>`.
- Set-based updates use `IUpdateSpecification<TEntity>`.
- Deletes use repository `RemoveAsync(ISpecification<TEntity>)`.
- No `IQueryable` crosses the operation/repository boundary.

## Migrations

- Put raw SQL files under `<Project>.Migration/Scripts/<Provider>/...`.
- Filenames start with Unix-seconds prefixes, for example `1761000000_create_foos.sql`.
- Once merged, never rename a script because the migration journal keys on filenames.
- Never use `EnsureCreated` or EF Core migrations in production.
