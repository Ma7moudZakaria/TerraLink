# ADR Review Checklist

Use this checklist in Phase 3 of the ADR workflow. Present only failures and notable strengths — do not dump the full list at the human.

## Framing

- [ ] Title is a verb phrase ("Choose X", "Adopt Y", "Replace Z with W")
- [ ] Status is set (`proposed`, `accepted`, etc.)
- [ ] Date is set

## Context

- [ ] Context explains *why* the decision is needed now
- [ ] Context is specific — a future reader with no prior knowledge can understand it
- [ ] Related ADRs or constraints are referenced
- [ ] No tribal knowledge assumed

## Decision

- [ ] Decision section states the chosen option precisely
- [ ] Decision explains why this option over alternatives (not just what)
- [ ] Decision is actionable — an agent can implement against it

## Options (MADR template only)

- [ ] At least two real options are documented
- [ ] Each option's trade-offs are stated
- [ ] The rejected options explain why they were rejected

## Non-Goals

- [ ] Non-goals are explicitly stated
- [ ] Non-goals prevent plausible scope creep

## Implementation Plan

- [ ] Specific file paths and directories are named
- [ ] The pattern all future code should follow is described
- [ ] Steps are ordered and actionable
- [ ] Test files or test cases are specified

## Verification

- [ ] Each verification criterion is a checkbox
- [ ] Each criterion is checkable by an agent (not "it works" — be specific)
- [ ] Criteria cover the implementation plan steps

## Consequences

- [ ] Positive consequences are listed
- [ ] Trade-offs / negative consequences are honest
- [ ] Follow-up tasks are identified

## Self-Containment

- [ ] The ADR can be handed to a new agent with no prior context and they can implement it
- [ ] No section contains placeholder text
- [ ] No "TBD" or "to be defined"
