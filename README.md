# CoorB AI Kit Starter - .NET

Copy these files into the root of a new or existing project. Keep existing project-specific instructions and append the generated agent files when needed.

## Native Agent Layouts

The generated ZIP uses each selected agent's native project layout by default:

- Codex: `AGENTS.md`, project skills in `.agents/skills/<skill-name>/SKILL.md`, and optional local MCP in `.codex/config.toml`.
- Claude Code: `CLAUDE.md`, project skills in `.claude/skills/<skill-name>/SKILL.md`, and optional local MCP in `.mcp.json`.
- GitHub Copilot: `.github/copilot-instructions.md`, repository instructions in `.github/instructions/*.instructions.md`, and optional local MCP in `.vscode/mcp.json`.

## Included Standards

- `adr-skill` - Architecture Decision Record workflow and templates.
- `free-package-licenses` - Dependency license policy for any third-party package choice.
- `dotnet-arch-skill` - Primary .NET 10 Clean Architecture and vertical-slice standards.
- `dotnet-lowcodehub-skill` - Routing and hard rules for LowCodeHub packages used by .NET projects.

## Required Tool Setup

### OctoGraph.Cli

Local code knowledge graph CLI and MCP stdio server.

OctoGraph.Cli should be installed globally once. The starter can include agent-native project-local MCP config files for selected agents, or you can rely on existing user/HOME MCP config.

From the repository root:

```powershell
octograph --version
octograph index .
```

If the global CLI is missing or stale, run one of these from any directory:

```powershell
dotnet tool install --global OctoGraph.Cli --version 0.0.3
dotnet tool update --global OctoGraph.Cli --version 0.0.3
```
The MCP server command used by generated agent-local config files is:

```powershell
octograph mcp
```

## Codex

Use `AGENTS.md` as the project instruction file. Codex discovers the selected standards from `.agents/skills/`.

## Claude

Use `CLAUDE.md` as the Claude Code project instruction file. Claude Code discovers the selected standards from `.claude/skills/`.

## GitHub Copilot

Use `.github/copilot-instructions.md` for repository-wide custom instructions. Copilot also receives selected standards through `.github/instructions/*.instructions.md`.
