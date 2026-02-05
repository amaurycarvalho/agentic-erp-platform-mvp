# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Agent.Api/Agent.Api.csproj src/Agent.Api/
COPY src/Agent.Application/Agent.Application.csproj src/Agent.Application/
COPY src/Agent.Domain/Agent.Domain.csproj src/Agent.Domain/
COPY src/Agent.Infrastructure/Agent.Infrastructure.csproj src/Agent.Infrastructure/

RUN dotnet restore src/Agent.Api/Agent.Api.csproj

COPY src/ .

WORKDIR /src/src/Agent.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Agent.Api.dll"]

