# Testing Architecture

## Default Split

```text
tests/
  <Project>.UnitTests/
  <Project>.IntegrationTests/
```

## Unit Tests

Test in isolation — no database, no HTTP, no file system.

Use for:
- domain rules
- value objects
- application handlers (with mocked infrastructure)
- validators
- edge cases and boundary conditions

## Integration Tests

Test real behavior across layers.

Use for:
- API endpoint behavior (routing, request/response, status codes)
- authorization behavior
- EF Core persistence (using a real test database or SQLite in-memory)
- migrations (smoke test that migrations apply cleanly)
- provider boundaries using safe test credentials or recorded responses

## Rules

- Unit tests do not hit databases, external APIs, or the file system.
- Integration tests use isolated test databases — never production.
- Each test is independent. Tests do not share state across runs.
- Add a regression test for every bug fix. The test should fail before the fix and pass after.

## Naming

```
MethodName_StateUnderTest_ExpectedBehavior
CreateInvoice_WhenAmountIsNegative_ThrowsDomainException
```

Or use descriptive sentences for integration tests:
```
POST /invoices returns 400 when amount is missing
```
