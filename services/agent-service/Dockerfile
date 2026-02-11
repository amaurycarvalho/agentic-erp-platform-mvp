# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/Agent.Api/Agent.Api.csproj src/Agent.Api/
COPY src/Agent.Application/Agent.Application.csproj src/Agent.Application/
COPY src/Agent.Domain/Agent.Domain.csproj src/Agent.Domain/
COPY src/Agent.Infrastructure/Agent.Infrastructure.csproj src/Agent.Infrastructure/

RUN dotnet restore src/Agent.Api/Agent.Api.csproj

COPY src/ src/

RUN dotnet publish src/Agent.Api/Agent.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
RUN useradd -m appuser

EXPOSE 8080

COPY --from=build /app/publish .
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Agent.Api.dll"]
