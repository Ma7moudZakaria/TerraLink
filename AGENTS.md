# Codex and Generic Agents Instructions

This project uses the CoorB AI Kit standards for .NET work.

## Native Standards

These standards are installed in this agent's native project layout:

- `.agents/skills/adr-skill/SKILL.md` - Architecture Decision Record workflow and templates.
- `.agents/skills/free-package-licenses/SKILL.md` - Dependency license policy for any third-party package choice.
- `.agents/skills/dotnet-arch-skill/SKILL.md` - Primary .NET 10 Clean Architecture and vertical-slice standards.
- `.agents/skills/dotnet-lowcodehub-skill/SKILL.md` - Routing and hard rules for LowCodeHub packages used by .NET projects.

## Required Tools

OctoGraph is expected to be installed globally for the current user. When project-local MCP support is included, this agent receives its own native MCP config file.

- Verify the global tool: `octograph --version` - Confirms OctoGraph.Cli is available from the current user's PATH.
- Build the repository graph: `octograph index .` - Indexes the current repository so agents can ask for graph-backed context.

## Working Rules

1. Identify the task type and use the matching native skill or instruction file.
2. Use the required context tool when available before opening unrelated files.
3. Keep project-specific instructions above these standards if this file is appended to an existing project.
4. Implement, test, and report changed files plus any open risk.
