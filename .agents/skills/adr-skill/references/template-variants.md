# Template Variants

## Quick Decision Guide

| Scenario | Template |
|----------|----------|
| One clear winner, few trade-offs | `adr-simple.md` |
| Multiple real options, structured comparison | `adr-madr.md` |
| Not sure | Use `adr-simple.md` first; upgrade to MADR if options become complex |

---

## `adr-simple.md` — Lean Template

**Use when:**
- The decision is already made and you're documenting it
- There is one clearly superior option
- The trade-offs are straightforward
- The team agrees on direction; you just need the implementation plan and verification

**Sections:** Context → Decision → Non-Goals → Implementation Plan → Verification → Consequences

**Typical length:** 1–2 pages

---

## `adr-madr.md` — MADR-Style Template

**Use when:**
- Two or more real options exist with meaningful trade-offs
- The decision is controversial or requires stakeholder buy-in
- You want to preserve the reasoning for rejected options (valuable for future agents)
- The decision has multiple decision drivers that weight options differently

**Sections:** Context → Decision Drivers → Options → Decision Outcome → Pros/Cons per Option → Implementation Plan → Verification

**Typical length:** 2–4 pages

---

## Common Mistakes

### Using MADR when simple would do

If you have one obvious choice and you're listing fake alternatives just to fill the template, use `adr-simple.md`.

### Using simple when options matter

If future agents might wonder "why didn't they use X?" — document X's trade-offs in MADR format. The cost of a longer ADR is much lower than the cost of re-litigating decisions.

### Skipping the Implementation Plan

This is the most common gap. An ADR without an Implementation Plan is a history document, not an executable specification. Always fill it.
