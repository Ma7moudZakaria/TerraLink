# Solution Layout

Default layout for new ASP.NET projects:

```text
<Project>.slnx
src/
  <Project>.API/
    Program.cs
    InfrastructureExtensions.cs
    appsettings.json
    Configurations/
    Services/

  <Project>.Domain/
    Entities/
    ValueObjects/
    Enums/
    Persistence/
      <Project>DbContext.cs
      Configurations/
    Abstracts/

  <Project>.Migration/
    IMigrationScanner.cs
    Scripts/
      SqlServer/
      PostgreSql/

  Integrations/
    <Project>.Integrations.<Provider>/

  UseCases/
    <Project>.UseCase.<Context>/
      I<Context>Scanner.cs
      <Context>UseCaseExtensions.cs
      Abstracts/
        Services/
        Repositories/
      Implementation/
        Services/
        Repositories/
      Features/
        <Feature>/
          <Feature>Module.cs
          Endpoints/
          Operations/
          Specifications/

tests/
  <Project>.UnitTests/
  <Project>.IntegrationTests/

docs/
  architecture/
  adr/
  standards/
```

## Existing Projects

If an existing project uses different names, map responsibilities to the existing folders. Do not rename without an explicit migration request.

## Non-Negotiables

- UseCase projects live under `src/UseCases/`.
- Integration projects live under `src/Integrations/`.
- The API project is a host only. It does not own feature endpoints.
- Domain owns entities, value objects, DbContext, and EF configurations for this blueprint.
- Migration scripts are raw SQL; EF Core migrations are not the default.
