# Mcp.Integration.Tests

Testes de integração da cadeia `mcp-service` -> `erp-acl-service`.

## Pré-requisitos

1. Subir serviços necessários:
   - `erp-acl-service`
   - `mcp-service`
2. Garantir endpoint de saúde HTTP:
   - `http://localhost:8084/health` (ERP ACL HTTP)
   - `http://localhost:8082/health` (MCP)
3. Garantir conectividade gRPC MCP -> ACL:
   - Se MCP estiver em Docker, usar `http://erp-acl-service:8081`.
   - Se MCP estiver local, usar `http://localhost:8081`.

## Modo recomendado (Docker Compose)

```bash
docker-compose up -d erp-acl-service mcp-service
curl -fs http://localhost:8084/health
curl -fs http://localhost:8082/health
MCP_BASE_URL=http://localhost:8082 dotnet test services/mcp-service/tests/Mcp.Integration.Tests/Mcp.Integration.Tests.csproj
```

Observação: no Compose, o `mcp-service` deve ter `ErpAcl__GrpcAddress=http://erp-acl-service:8081` para não depender de `localhost` quando `ASPNETCORE_ENVIRONMENT=Development`.

## Modo local (dotnet run)

Suba primeiro o ERP ACL:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run --project services/erp-acl-service/src/ErpAcl.Api/ErpAcl.Api.csproj
```

Em outro terminal, suba o MCP forçando o endereço do ACL:

```bash
ASPNETCORE_ENVIRONMENT=Development ErpAcl__GrpcAddress=http://localhost:8081 dotnet run --project services/mcp-service/src/Mcp.Api/Mcp.Api.csproj
```

Depois execute os testes:

```bash
MCP_BASE_URL=http://localhost:8082 dotnet test services/mcp-service/tests/Mcp.Integration.Tests/Mcp.Integration.Tests.csproj
```

## Execução

```bash
dotnet test services/mcp-service/tests/Mcp.Integration.Tests/Mcp.Integration.Tests.csproj
```

Se necessário, configurar URL base do MCP:

```bash
MCP_BASE_URL=http://localhost:8082 dotnet test services/mcp-service/tests/Mcp.Integration.Tests/Mcp.Integration.Tests.csproj
```

## Troubleshooting

Se o teste retornar `503` com `acl_unavailable`:

1. O MCP está ativo, mas não consegue conectar ao ACL gRPC;
2. Verifique se o ACL está ouvindo em `8081` (gRPC);
3. Verifique se o MCP está com `ErpAcl__GrpcAddress` correto para o modo de execução.

## Rastreabilidade

- REQ: `REQ-FUNC-003`, `REQ-FUNC-004`, `REQ-FUNC-005`
- UC: `UC-MCP-001`, `UC-MCP-002`
