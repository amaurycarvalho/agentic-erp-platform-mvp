## Why

`specifics/plan.md` ("Fase seguinte") and the MCP governance rules call for evolving the MCP tool catalog beyond the MVP's two tools, adding per-tool security/authorization, and formalizing the contract versioning policy that governs incompatible changes. The MVP (`mcp` capability) is implemented and archived; this change grows it.

## What Changes

- Extend the MCP tool catalog with new ERP capabilities (beyond `erp.create_order` and `erp.cancel_invoice`).
- Introduce per-tool authorization so execution is allowed only for callers authorized for that tool.
- Formalize the tool contract versioning policy (incompatible changes require a new tool version).
- Preserve the existing catalog-gating, validation, and error-taxonomy behavior.

## Capabilities

### New Capabilities
<!-- No new capability introduced; this evolves the existing mcp capability. -->

### Modified Capabilities
- `mcp`: adds catalog extensibility, per-tool authorization, and tool contract versioning requirements.

## Impact

- Extends the `mcp-service` catalog, its authorization layer, and its contract versioning.
- May add new gRPC ACL contracts and MCP tool definitions.
- Adds authorization checks to the execution endpoint.
