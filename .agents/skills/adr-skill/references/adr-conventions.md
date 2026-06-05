# ADR Conventions

## Directory

Default: `docs/adr/`

Alternatives (accept if already present in the repo):
- `docs/decisions/`
- `adr/`
- `contributing/decisions/`

Once chosen, do not change the directory without updating all existing ADR cross-references.

## Filename Strategy

### Numeric prefix (default for new repos)

```
0001-choose-database.md
0002-adopt-cqrs.md
```

Pad to 4 digits. Increment from the highest existing number.

### Date prefix (use if the repo already uses this)

```
2026-05-10-choose-database.md
```

### Slug-only (avoid for new repos — hard to order)

```
choose-database.md
```

## Title Format

Use a verb phrase:
- `Choose database for persistence`
- `Adopt CQRS for application layer`
- `Replace Redis with in-process cache`

Avoid noun phrases:
- ~~`Database decision`~~
- ~~`CQRS`~~

## Status Values

| Value | Meaning |
|-------|---------|
| `proposed` | Under review |
| `accepted` | Active — agents must follow this |
| `rejected` | Not adopted — kept for historical record |
| `deprecated` | No longer applies; see ADR for reason |
| `superseded-by: XXXX` | Replaced by ADR XXXX |

Use `scripts/set_adr_status.js` to update status without rewriting history.

## Superseding

When a new ADR replaces an old one:
1. Create the new ADR. Set status to `proposed`.
2. In the new ADR, reference the old one: "Supersedes ADR 0003."
3. Once the new ADR is accepted, update the old one: status → `superseded-by: 0007`.
4. Link both ways.

## Index

Maintain `docs/adr/README.md` (or `index.md`) with a table of all ADRs:

```markdown
| # | Title | Status | Date |
|---|-------|--------|------|
| 0001 | Choose database | accepted | 2026-05-10 |
```

## Lifecycle

```
proposed → accepted → (deprecated or superseded)
proposed → rejected
```

Never delete an ADR. History is the point.
