FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
RUN useradd -m appuser
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first to maximize layer cache reuse on restore.
COPY src/ErpAcl.Api/ErpAcl.Api.csproj src/ErpAcl.Api/
COPY src/ErpAcl.Application/ErpAcl.Application.csproj src/ErpAcl.Application/
COPY src/ErpAcl.Domain/ErpAcl.Domain.csproj src/ErpAcl.Domain/
COPY src/ErpAcl.Infrastructure/ErpAcl.Infrastructure.csproj src/ErpAcl.Infrastructure/
COPY src/ErpAcl.Contracts/ErpAcl.Contracts.csproj src/ErpAcl.Contracts/
COPY src/ErpAcl.Contracts/Protos src/ErpAcl.Contracts/Protos/

RUN dotnet restore src/ErpAcl.Api/ErpAcl.Api.csproj

COPY src/ src/
RUN dotnet publish src/ErpAcl.Api/ErpAcl.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN chown -R appuser:appuser /app
USER appuser
ENTRYPOINT ["dotnet", "ErpAcl.Api.dll"]
