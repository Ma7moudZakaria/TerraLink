

<!-- OCTOGRAPH:START -->
# OctoGraph

Use OctoGraph before making code changes that require repository context, impact analysis, API route discovery, tool discovery, or cross-file navigation.

Repository:
- Name: `Backend`
- Path: `G:\source\RealEstateDevelopment\Backend`
- Indexed files: `429`
- Symbols: `7642`
- Edges: `11827`
- Stale: `no`

How to use OctoGraph:
- For any question about repository structure, symbols, APIs, dependencies, impact, auth or business flow, or where logic lives, call OctoGraph first.
- Discovery workflow: call `query` with `mode: hybrid`, inspect the top results, then call `context` with `pack: true` on the best symbol/file before answering or editing.
- Change workflow: call `context` for the target, then `impact` before editing shared symbols, handlers, DTOs, API contracts, config, or tool entry points.
- API workflow: use `route_map` for route discovery, `shape_check` for contract drift, and `api_impact` for affected route/shape context.
- Graph workflow: use typed graph tools (`query`, `context`, `impact`, `route_map`, `tool_map`, `api_impact`) instead of raw graph-query languages.
- Use OctoGraph MCP tools directly when your editor exposes them.
- Do not inspect `.octograph` files manually; they are storage and generated artifacts, not the public interface.
- Use grep/file search only after OctoGraph points you to relevant files, or if OctoGraph is unavailable or returns no useful result.
- Do not answer by printing these CLI commands; call the tool or command, read the result, and answer from that evidence.
- If MCP is unavailable, run the CLI fallback internally and summarize the result.

MCP tools:
- `status` with `{ "repo": "Backend" }` checks whether the local graph is stale.
- `query` with `{ "repo": "Backend", "query": "<topic>", "mode": "hybrid", "limit": 10 }` finds relevant symbols and files.
- `context` with `{ "repo": "Backend", "target": "<symbol-or-file>", "pack": true, "budget": 4000 }` creates a compact working context.
- `impact` with `{ "repo": "Backend", "target": "<symbol-or-file>" }` lists likely downstream effects.
- `route_map` with `{ "repo": "Backend" }` lists indexed API routes.
- `tool_map` with `{ "repo": "Backend" }` lists indexed local tools and scripts.
- `shape_check` with `{ "repo": "Backend", "target": "<route-or-shape>" }` checks API response shape consistency.
- `api_impact` with `{ "repo": "Backend", "target": "<route-or-handler>" }` combines route and shape impact.

CLI fallback:
- `octograph status Backend`
- `octograph query "<topic>" --repo Backend --mode hybrid --limit 10`
- `octograph context "<symbol-or-file>" --repo Backend --pack --budget 4000`
- `octograph impact "<symbol-or-file>" --repo Backend`
- `octograph route-map --repo Backend`
- `octograph tool-map --repo Backend`
- `octograph shape-check --repo Backend`
- `octograph api-impact "<route-or-handler>" --repo Backend`

Workflow:
- If `status` reports stale, run `octograph index` from the repository root before relying on graph answers.
- Prefer `context --pack` for implementation work and `impact` before editing shared symbols, handlers, DTOs, API contracts, or tool entry points.
- When results look weak, retry `query` with a more specific symbol, file path, endpoint, config key, or error text before falling back to raw file search.
- Use `octograph mcp` for editor or agent integrations that support MCP stdio servers.

Indexed routes:
- `GET /` in `UseCases/TerraLink.UseCase.Asset/Applications/Features/Buildings/Endpoints/Get/GetBuildingsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Asset/Applications/Features/Lands/Endpoints/Get/GetLandsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Asset/Applications/Features/Units/Endpoints/Get/GetUnitsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Finance/Features/IncomingPayments/Endpoints/GetIncomingPaymentsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Finance/Features/Installments/Endpoints/GetInstallmentsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Finance/Features/OutgoingPayments/Endpoints/GetOutgoingPaymentsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.IdentityShield/Applications/Features/Endpoints/Roles/GetAllRoles/GetAllRolesEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.IdentityShield/Applications/Features/Endpoints/Users/Get/GetUsersEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Sales/Applications/Features/Clients/Endpoints/Get/GetAllClientsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Sales/Applications/Features/Contracts/Endpoints/Get/GetAllContractsEndpoint.cs`
- `GET /` in `UseCases/TerraLink.UseCase.Sales/Applications/Features/FollowUpCalls/Endpoints/Get/GetAllFollowUpCallsEndpoint.cs`
- `POST /` in `UseCases/TerraLink.UseCase.Finance/Features/IncomingPayments/Endpoints/CreateIncomingPaymentEndpoint.cs`
- `POST /` in `UseCases/TerraLink.UseCase.Finance/Features/Installments/Endpoints/CreateInstallmentEndpoint.cs`
- `POST /` in `UseCases/TerraLink.UseCase.Finance/Features/OutgoingPayments/Endpoints/CreateOutgoingPaymentEndpoint.cs`
- `POST /` in `UseCases/TerraLink.UseCase.IdentityShield/Applications/Features/Endpoints/Roles/CreateRole/CreateRoleEndpoint.cs`
- `POST /` in `UseCases/TerraLink.UseCase.Sales/Applications/Features/FollowUpCalls/Endpoints/Create/CreateFollowUpCallEndpoint.cs`
- `GET /contracts/{contractId:guid}/installments` in `UseCases/TerraLink.UseCase.Finance/Features/IncomingPayments/Endpoints/GetContractInstallmentsPaymentStatusEndpoint.cs`
- `POST /create` in `UseCases/TerraLink.UseCase.Asset/Applications/Features/Buildings/Endpoints/Create/CreateBuildingEndpoint.cs`
- `POST /create` in `UseCases/TerraLink.UseCase.Asset/Applications/Features/Lands/Endpoints/Create/CreateLandEndpoint.cs`
- `POST /create` in `UseCases/TerraLink.UseCase.Asset/Applications/Features/Units/Endpoints/Create/CreateUnitEndpoint.cs`

Indexed tools:
- None found in the current graph.
<!-- OCTOGRAPH:END -->
