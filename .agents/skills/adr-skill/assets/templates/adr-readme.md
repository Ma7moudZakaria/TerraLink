# Architecture Decision Records

This directory contains the architectural decision records for this project.

## What is an ADR?

An ADR captures a significant decision made during this project's lifetime. Each ADR documents:
- The context that made the decision necessary
- The options that were considered
- The decision that was made and why
- The implementation plan for agents to follow
- Verification criteria to confirm correct implementation

## Status Values

| Status | Meaning |
|--------|---------|
| `proposed` | Under consideration, not yet approved |
| `accepted` | Active decision — code should follow this |
| `rejected` | Considered but not adopted |
| `deprecated` | No longer applies; see note in the ADR |
| `superseded-by: XXXX` | Replaced by a newer ADR |

## Index

| # | Title | Status | Date |
|---|-------|--------|------|
| — | *(no ADRs yet — run `scripts/bootstrap_adr.js` to create the first one)* | — | — |

## Creating a New ADR

Use the `adr-skill` and its four-phase workflow, or run:

```bash
node /path/to/adr-skill/scripts/new_adr.js --title "Your decision title" --status proposed
```

## Conventions

See `references/adr-conventions.md` in the adr-skill for naming, status, and lifecycle rules.
