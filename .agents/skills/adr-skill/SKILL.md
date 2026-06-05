---
name: adr-skill
description: >
  Create, update, review, and consult Architecture Decision Records (ADRs)
  optimized for agentic coding workflows. ADRs created with this skill are
  executable specifications — a human approves the decision, an agent
  implements it. Use when proposing, writing, accepting, superseding, or
  consulting an ADR; bootstrapping an ADR folder; or enforcing ADR conventions
  in a codebase. Uses a Socratic interview to capture intent before drafting,
  and validates output against an agent-readiness checklist.
applyTo: "**"
---

# ADR Skill

## Philosophy

ADRs created with this skill are **executable specifications for coding agents**. A human approves the decision; an agent implements it. The ADR must contain everything the agent needs to write correct code without asking follow-up questions.

This means:
- Constraints are explicit and measurable, not vague
- Decisions are specific enough to act on ("use PostgreSQL 16 with pgvector", not "use a database")
- Consequences map to concrete follow-up tasks
- Non-goals are stated to prevent scope creep
- The ADR is self-contained — no tribal knowledge assumptions
- **The ADR includes an implementation plan** — which files to touch, which patterns to follow, which tests to write, and how to verify the decision was implemented correctly

---

## Seeded ADRs — Active Constraints

The following ADRs are already accepted in this project. Treat them as live constraints before proposing or implementing anything new.

| # | Title | Status | Key constraint |
|---|-------|--------|----------------|
| [0001](docs/adr/0001-use-clean-architecture-for-aspnet.md) | Use Clean Architecture for ASP.NET | accepted | Api / Application / Domain / Infrastructure layering; business logic never in API or Infrastructure |
| [0002](docs/adr/0002-use-agent-specific-copy-folders.md) | Use agent-specific copy folders | accepted | One folder per agent (`claude/`, `codex/`, `github/`); intentional duplication is acceptable |
| [0003](docs/adr/0003-use-octograph-cli-for-ai-context.md) | Use OctoGraph.Cli for AI context | accepted | Agents restore `OctoGraph.Cli`, keep the repository indexed with `octograph index`, and use `octograph mcp` for graph-backed context; no vector DB or durable memory |

Any decision that contradicts these must supersede them via a new ADR — do not silently work around them.

---

## OctoGraph Integration

Before scanning the codebase manually, use OctoGraph if it is available. OctoGraph gives local graph-backed repository context that is far cheaper than broad file search.

### Phase 0 — Graph-first context gathering

```
1. octograph --version
   -> confirms OctoGraph.Cli is installed globally for the current user

   If unavailable, install it once with:
   `dotnet tool install --global OctoGraph.Cli --version 0.0.3`

2. octograph index .
   -> creates or refreshes the local repository graph

3. Use the configured OctoGraph MCP server
   -> user/HOME or project-local agent MCP config should run `octograph mcp`

4. Use the OctoGraph MCP context/search tools exposed by your client
   -> use the returned file list as the set to scan for existing ADRs and related code

5. Read only the files returned. Do not explore beyond them unless something is clearly missing.
```

### When OctoGraph MCP is unavailable

State the limitation once, then fall back:

```
OctoGraph MCP is unavailable or the MCP server is not running.
Falling back to file_search for ADR discovery and codebase scan.
```

Do not ask the human to fix the MCP before continuing. Fall back and proceed.

---

## When to Write an ADR

Write an ADR when a decision:
- Changes how the system is built or operated (new dependency, architecture pattern, infrastructure choice, API design)
- Is hard to reverse once code is written against it
- Affects other people or agents who will work in this codebase
- Has real alternatives that were considered and rejected

Do NOT write an ADR for:
- Routine implementation choices within an established pattern
- Bug fixes or typo corrections
- Decisions already captured in an existing ADR (update it instead)
- Style preferences covered by linters or formatters

When in doubt: if a future agent working in this codebase would benefit from knowing *why* this choice was made, write the ADR.

### Proactive ADR Triggers (For Agents)

Stop and propose an ADR before continuing if you are about to:
- Introduce a new dependency that doesn't already exist in the project
- Create a new architectural pattern (new error handling approach, new data access layer, new API convention) that other code will need to follow
- Make a choice between two or more real alternatives where the tradeoffs are non-obvious
- Change something that contradicts an existing accepted ADR
- Write a long code comment explaining "why" — that reasoning belongs in an ADR

**How to propose**: Tell the human what decision you've hit, why it matters, and ask if they want to capture it as an ADR. If yes, run the four-phase workflow. If no, note the decision in a code comment and move on.

---

## Creating an ADR: Four-Phase Workflow

Every ADR goes through four phases. Do not skip phases.

### Phase 0: Scan the Codebase

Before asking any questions, gather context:

1. **Try OctoGraph first.**
   ```
   octograph --version
   # If unavailable:
   dotnet tool install --global OctoGraph.Cli --version 0.0.3
   octograph index .
   # Agent user/HOME or project-local MCP config should run:
   octograph mcp
   ```
   Use the OctoGraph MCP context/search tools exposed by your client to find files relevant to the decision area. If OctoGraph MCP is unavailable, fall back to `file_search` and `grep_search`. State the limitation once and continue.

2. **Find existing ADRs.** Check `docs/adr/`, `docs/decisions/`, `adr/`, `contributing/decisions/`. Cross-reference with the Seeded ADRs table above. Note:
   - Existing conventions (directory, naming, template style)
   - Decisions that relate to or constrain the current one
   - ADRs this new decision might supersede

3. **Check the tech stack.** Read `*.csproj`, `package.json`, `go.mod`, or equivalent. Note relevant dependencies and versions.

4. **Find related code patterns.** Use the graph-returned files as the starting set. Identify patterns that will be affected by the decision.

5. **Check for ADR references in code.** Look for ADR references in comments and docs. This reveals which existing decisions govern which parts of the codebase.

6. **Carry this context into Phase 1.** It will sharpen your questions and prevent contradicting existing decisions.

---

### Phase 1: Capture Intent (Socratic)

Interview the human to understand the decision space. Ask questions **one at a time**, building on previous answers.

**Core questions** (ask in roughly this order, skip what's already clear from Phase 0):

1. **What are you deciding?** — Get a short, specific title. Push for a verb phrase ("Choose X", "Adopt Y", "Replace Z with W").
2. **Why now?** — What broke, what's changing, or what will break if you do nothing?
3. **What constraints exist?** — Tech stack, timeline, budget, existing code, compliance. Be concrete.
4. **What does success look like?** — Measurable outcomes. Push past "it works" to specifics.
5. **What options have you considered?** — At least two. For each: what's the core tradeoff?
6. **What's your current lean?** — Capture gut intuition early.
7. **Who needs to know or approve?** — Decision-makers, consulted experts, informed stakeholders.
8. **What would an agent need to implement this?** — Which files? Which patterns to follow? Which tests would prove it's working?

**Adaptive follow-ups:**
- "What's the worst-case outcome if this decision is wrong?"
- "What would make you revisit this in 6 months?"
- "Is there anything you're explicitly choosing NOT to do?"
- "I found [existing ADR/pattern] — does this new decision interact with it?"

**Intent Summary Gate**: Before moving to Phase 2, present a structured summary:

> **Here's what I'm capturing for the ADR:**
>
> - **Title**: {title}
> - **Trigger**: {why now}
> - **Constraints**: {list}
> - **Options**: {option 1} vs {option 2}
> - **Lean**: {which option and why}
> - **Non-goals**: {what's explicitly out of scope}
> - **Related ADRs/code**: {what exists that this interacts with}
> - **Affected files/areas**: {where in the codebase this lands}
> - **Verification**: {how we'll know it's implemented correctly}
>
> **Does this capture your intent? Anything to add or correct?**

Do NOT proceed to Phase 2 until the human confirms the summary.

---

### Phase 2: Draft the ADR

1. **Choose the ADR directory.** If one exists (found in Phase 0), use it. If none exists, create `docs/adr/` by default or run `scripts/bootstrap_adr.js`.

2. **Choose a filename strategy.**
   - If existing ADRs use numeric prefixes (`0001-...`), continue that.
   - If they use date prefixes (`2026-05-10-...`), continue that.
   - If none exist, use numeric prefixes.

3. **Choose a template.**
   - Use `assets/templates/adr-simple.md` for straightforward decisions (one clear winner, minimal tradeoffs).
   - Use `assets/templates/adr-madr.md` for decisions with multiple options and structured pros/cons.
   - See `references/template-variants.md` for guidance.

4. **Fill every section from the confirmed intent summary.** No placeholder text. Every section contains real content or is removed (optional sections only).

5. **Write the Implementation Plan.** This is the most critical section. It tells the next agent exactly what to do — which files to touch, which patterns to follow, which tests to write.

6. **Write Verification criteria as checkboxes.** Each item must be specific enough that an agent can check it programmatically or manually.

7. **Generate the file.** Copy a template from `assets/templates/`, fill it manually, and remove any unused optional sections.

---

### Phase 3: Review Against Checklist

After drafting, review against `references/review-checklist.md`.

**Present the review as a summary:**

> **ADR Review**
>
> ✅ **Passes**: {what's solid}
>
> ⚠️ **Gaps found**:
> - {specific gap — e.g., "Implementation Plan doesn't mention test files"}
>
> **Recommendation**: {Ship it / Fix the gaps first / Needs more Phase 1 work}

Only surface failures and notable strengths. Do not recite every passing checkbox.

If there are gaps, propose specific fixes. Do not finalize until the ADR passes the checklist or the human explicitly accepts the gaps.

---

## Consulting ADRs (Read Workflow)

Read existing ADRs **before implementing changes** in any codebase that has them.

### When to Consult ADRs

- Before starting work on a feature that touches architecture (auth, data layer, API design, infrastructure)
- When you encounter a pattern and wonder "why is it done this way?"
- Before proposing a change that might contradict an existing decision
- When a human says "check the ADRs" or "there's a decision about this"
- When you find an ADR reference in a code comment

### How to Consult ADRs

1. **Find the ADR directory.** Check `docs/adr/`, `docs/decisions/`, `adr/`, `contributing/decisions/`. Also check for an index file (`README.md` or `index.md`).
2. **Scan titles and statuses.** Focus on `accepted` ADRs — these are active decisions.
3. **Read relevant ADRs fully.** Read context, decision, consequences, non-goals, AND the Implementation Plan. The Implementation Plan tells you which patterns to follow.
4. **Respect the decisions.** If an accepted ADR says "use PostgreSQL", don't propose MongoDB without creating a new ADR that supersedes it.
5. **Follow the Implementation Plan.** When coding in an area governed by an ADR, follow the patterns it specifies.
6. **Reference ADRs in your work.** Add ADR references in code comments and PR descriptions.

---

## Code ↔ ADR Linking

### ADR → Code (in the Implementation Plan)

```markdown
## Implementation Plan

- **Affected paths**: `src/Infrastructure/Persistence/`, `src/Domain/Entities/`
- **Pattern**: all database queries go through the repository in `src/Infrastructure/Persistence/Repositories/`
- **Tests**: add integration tests in `tests/<Project>.IntegrationTests/Persistence/`
```

### Code → ADR (in comments)

```csharp
// ADR: Repository pattern for data access
// See: docs/adr/0001-use-clean-architecture-for-aspnet.md
public class InvoiceRepository : IInvoiceRepository
```

Keep references lightweight — one comment at the entry point, not on every line.

---

## Other Operations

### Update an Existing ADR

1. Identify the intent:
   - **Accept / reject**: change status, add final context.
   - **Deprecate**: status → `deprecated`, explain replacement path.
   - **Supersede**: create a new ADR, link both ways.
   - **Add learnings**: append to `## More Information` with a date stamp. Do not rewrite history.

2. Edit the ADR status in place and append dated context for any lifecycle change. Do not rewrite historical decision text.

### Post-Acceptance Lifecycle

After an ADR is accepted:
1. Create implementation tasks for each item in the Implementation Plan.
2. Reference the ADR in PR descriptions.
3. Add code references at key implementation points.
4. Walk through Verification checkboxes once implementation is complete.
5. Monitor for revisit conditions specified in the ADR.

### Index

If the repo has an ADR index (`README.md` or `index.md` in the ADR dir), keep it updated by adding or editing the relevant entry manually.

### Bootstrap

When introducing ADRs to a repo that has none, create `docs/adr/`, add a `README.md` index, and create the first ADR for "Adopt architecture decision records" using `assets/templates/adr-simple.md`.

---

## Resources

### references/
- `references/review-checklist.md` — agent-readiness checklist for Phase 3.
- `references/adr-conventions.md` — directory, filename, status, and lifecycle conventions.
- `references/template-variants.md` — when to use simple vs MADR-style templates.

### assets/templates/
- `assets/templates/adr-simple.md` — lean template for straightforward decisions.
- `assets/templates/adr-madr.md` — MADR-style template for decisions with multiple options.
- `assets/templates/adr-readme.md` — ADR index scaffold used by `bootstrap_adr.js`.
