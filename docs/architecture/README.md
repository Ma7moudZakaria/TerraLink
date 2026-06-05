# Architecture

The repository follows the company .NET architecture skill layout:

```text
src/
  TerraLink.API/
  TerraLink.Domain/
  TerraLink.Migration/
  Integrations/
  UseCases/
tests/
```

API is the composition host. UseCase projects own vertical slices. Domain owns entities, value objects, persistence, and shared abstractions. Migration owns raw SQL scripts.
