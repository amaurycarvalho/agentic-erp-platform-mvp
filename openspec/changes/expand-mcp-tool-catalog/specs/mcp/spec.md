## ADDED Requirements

### Requirement: Tool catalog is extensible

The mcp-service SHALL support adding new tools to the catalog without modifying existing tool behavior, so the catalog can grow beyond `erp.create_order` and `erp.cancel_invoice`.

#### Scenario: A new tool can be added
- **WHEN** a new ERP capability is registered in the catalog
- **THEN** it becomes discoverable and executable while existing tools remain unchanged

### Requirement: Per-tool authorization

Each catalogued tool SHALL declare its authorization requirement, and the mcp-service SHALL deny execution when the caller is not authorized for that specific tool.

#### Scenario: Unauthorized execution is denied
- **WHEN** a caller requests execution of a tool it is not authorized for
- **THEN** the mcp-service denies the execution and returns an authorization error

#### Scenario: Authorized execution proceeds
- **WHEN** a caller requests execution of a tool it is authorized for
- **THEN** the mcp-service proceeds with the tool execution

### Requirement: Tool contract versioning policy

The mcp-service SHALL version tool contracts explicitly and SHALL require that incompatible changes create a new tool version, preserving the existing governance rule that incompatible changes need a new contract version.

#### Scenario: Incompatible change creates a new version
- **WHEN** a tool contract undergoes an incompatible change
- **THEN** the tool is published under a new version
- **AND** the previous version remains available to existing consumers

#### Scenario: Compatible change does not require a new version
- **WHEN** a tool contract change is backward compatible
- **THEN** it may be applied without introducing a new tool version
