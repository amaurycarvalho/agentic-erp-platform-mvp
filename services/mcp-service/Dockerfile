FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Mcp.Api/Mcp.Api.csproj src/Mcp.Api/
COPY src/Mcp.Application/Mcp.Application.csproj src/Mcp.Application/
COPY src/Mcp.Domain/Mcp.Domain.csproj src/Mcp.Domain/
COPY src/Mcp.Infrastructure/Mcp.Infrastructure.csproj src/Mcp.Infrastructure/

RUN dotnet restore src/Mcp.Api/Mcp.Api.csproj

COPY src/ .

WORKDIR /src/src/Mcp.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8082

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mcp.Api.dll"]

