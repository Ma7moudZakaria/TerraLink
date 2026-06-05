# Observability

Every production ASP.NET project should make failures diagnosable without exposing sensitive data.

## Default Signals

- Structured logs (Serilog, OpenTelemetry, or the project's existing provider)
- Request correlation IDs on every HTTP request
- Health check endpoints
- Metrics (request duration, error rates, queue depth)
- Traces for external calls, queues, and heavy database workflows
- Safe, consistent error responses

## Rules

- Do not log secrets, tokens, passwords, connection strings, private keys, or raw customer data.
- Include enough context to reproduce and debug a failure.
- Use centralized exception handling (middleware or filters) — do not scatter try/catch at the endpoint level.
- Use Problem Details for API errors.
- Add distributed tracing around external provider calls, message queue operations, and database-heavy workflows.

## Log Levels

| Level | When |
|-------|------|
| `Debug` | Detailed flow during development |
| `Information` | Normal operation, lifecycle events |
| `Warning` | Unexpected but recoverable situation |
| `Error` | Failure that requires attention |
| `Critical` | Service-level failure |

Do not log `Error` for expected business rejections (validation failures, not-found). Use `Warning` or `Information`.

## AI Work

When an AI agent changes behavior, the task completion summary should include:
- files touched
- validation run
- risks identified
- any follow-up observability needs (new log points, traces, alerts)
