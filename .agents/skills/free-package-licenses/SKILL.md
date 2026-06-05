---
name: free-package-licenses
description: Company dependency policy for package/license safety. Use whenever Codex adds, updates, recommends, or reviews third-party dependencies such as NuGet, npm, PyPI, Maven, Docker images, SDKs, CLIs, analyzers, build tools, or transitive package replacements. Enforces automatic use only for company-approved free/permissive licenses and blocks packages with commercial, source-available, copyleft, unclear, custom, or changed license terms unless the user explicitly approves.
---

# Free Package Licenses

## Rule

Only add or recommend third-party packages automatically when their license is verified and approved for company use.

Approved licenses:

| SPDX / common name | Status |
|---|---|
| `MIT` | allowed |
| `Apache-2.0` / Apache License 2.0 | allowed |
| `BSD-2-Clause` | allowed |
| `BSD-3-Clause` | allowed |
| `ISC` | allowed |
| `Unlicense` | allowed |
| `CC0-1.0` | allowed for code/data where attribution and patent needs are not relevant |

Everything else is not automatic.

## Blocked Without Explicit Approval

- Commercial, paid, trial, dual-license, or per-seat packages.
- Source-available licenses, including BSL / BUSL, PolyForm, SSPL, Elastic License, Commons Clause, and similar.
- Copyleft licenses, including GPL, AGPL, LGPL, MPL, EPL, and CDDL.
- Custom licenses, ambiguous license text, missing license metadata, or packages whose repository/license page cannot be verified.
- Packages with known company policy issues, even if older versions were previously common. `MediatR` is not allowed automatically.
- Any package whose license changed recently or differs between package metadata and repository license file.

## Workflow

Before adding, updating, or recommending a dependency:

1. Prefer company/internal packages first, especially `LowCodeHub.*` for .NET capabilities covered by `dotnet-lowcodehub`.
2. Check the package metadata and repository license. For NuGet, inspect the `.csproj`, `.nuspec`, nuget.org metadata, or package license expression. For npm/PyPI/Maven, inspect registry metadata plus the repository `LICENSE`.
3. Confirm the license is in the approved list above.
4. If approved, proceed and mention the license in the final summary when a dependency was added.
5. If not approved or unclear, do not add it. Use an existing internal package, implement a small local solution, or ask the user for explicit approval with the package name, version, license, and reason it is needed.

## Review Standard

Treat license risk as a blocking code-review finding:

- A new dependency with no verified approved license is a blocker.
- A dependency replacement must be at least as license-safe as the removed package.
- Transitive dependencies do not need exhaustive manual review for routine use, but if a package is high-risk, source-available, commercial-adjacent, or security-sensitive, inspect its dependency tree before proceeding.
- Do not bypass this policy because a package is popular, free-of-charge, open source, or already familiar.

## Suggested Response When Blocked

Use concise technical wording:

```text
I can't add `PackageName` automatically because its license is `LicenseName`, which is outside the approved list. I can use `AllowedAlternative` instead, or you can explicitly approve this package/version.
```
