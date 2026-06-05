# Integration Architecture

External systems belong behind interfaces. Domain knows nothing about them.

## Default Shape

```text
src/<Project>.Domain/
  Abstracts/
    IEmailSender.cs
    IPaymentProvider.cs

src/Integrations/
  <Project>.Integrations.Email/
    SendGridEmailSender.cs
  <Project>.Integrations.Payments/
    StripePaymentProvider.cs
```

## Rules

- Domain defines cross-cutting abstractions. A UseCase may define a context-local abstraction when no other context should consume it.
- Integration projects implement abstractions.
- Domain does not know about HTTP clients, SDKs, provider DTOs, or secrets.
- Provider options are bound from configuration using strongly-typed options classes.
- Provider failures are caught and translated into domain/use-case failures — do not let SDK exceptions leak into UseCase code.
- Sensitive payloads (tokens, keys, card data) must not be logged.

## Options Classes

```text
<Project>.Integrations.<Provider>/Options/
  SendGridOptions.cs
  StripeOptions.cs
```

Validate required options at startup using `IValidateOptions<T>` or `ValidateOnStart()`.

## Testing

- Use fakes or mocks for unit tests of UseCase operation handlers.
- Use integration tests for real provider boundaries only when safe test credentials and test environments exist.
- Never hit production APIs in automated tests.
