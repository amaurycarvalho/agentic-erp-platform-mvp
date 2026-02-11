FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY services/mcp-service/src/Mcp.Api/Mcp.Api.csproj services/mcp-service/src/Mcp.Api/
COPY services/mcp-service/src/Mcp.Application/Mcp.Application.csproj services/mcp-service/src/Mcp.Application/
COPY services/mcp-service/src/Mcp.Domain/Mcp.Domain.csproj services/mcp-service/src/Mcp.Domain/
COPY services/mcp-service/src/Mcp.Infrastructure/Mcp.Infrastructure.csproj services/mcp-service/src/Mcp.Infrastructure/
COPY services/erp-acl-service/src/ErpAcl.Contracts/ErpAcl.Contracts.csproj services/erp-acl-service/src/ErpAcl.Contracts/
COPY services/erp-acl-service/src/ErpAcl.Contracts/Protos services/erp-acl-service/src/ErpAcl.Contracts/Protos

RUN dotnet restore services/mcp-service/src/Mcp.Api/Mcp.Api.csproj

COPY services/mcp-service/src/ services/mcp-service/src/
COPY services/erp-acl-service/src/ErpAcl.Contracts/ services/erp-acl-service/src/ErpAcl.Contracts/

WORKDIR /src/services/mcp-service/src/Mcp.Api
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
RUN useradd -m appuser

EXPOSE 8082

COPY --from=build /app/publish .
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Mcp.Api.dll"]
