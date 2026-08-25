## Context

The MVP `mcp` capability is implemented and archived: an explicit in-memory tool catalog gates execution, payload validation runs before the ACL call, and the error taxonomy (`tool_not_found`, `validation_error`, `acl_business_error`, `acl_unavailable`) is in place. This change evolves the catalog, adds authorization, and formalizes versioning.

## Goals / Non-Goals

**Goals:**
- Make the catalog extensible without destabilizing existing tools.
- Add per-tool authorization gates at the execution boundary.
- Define and enforce a tool contract versioning policy.

**Non-Goals:**
- No change to the existing `erp.create_order` / `erp.cancel_invoice` contracts.
- No change to the ACL v1 gRPC contract.
- No change to the error taxonomy or catalog gating behavior already implemented.

## Decisions

- **Catalog as configuration + domain contract**: tool schemas and their authorization requirements live beside the catalog; adding a tool is a catalog addition, not a code branch.
- **Authorization at the execution boundary**: the authorization requirement is enforced in the `ExecuteTool` use case before invoking the ACL, returning an authorization error for denial.
- **Explicit version fields on tool contracts**: each tool exposes a version; incompatible changes bump the version and coexist under a distinct name/id.

## Risks / Trade-offs

- [Broadening the catalog increases attack surface] → Mitigation: per-tool authorization defaults to deny, and exposure only for explicitly registered tools.
- [Version coexistence doubles contract maintenance] → Mitigation: coexist only until consumers migrate, then deprecate the old version.
