FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN useradd -m appuser
USER appuser
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/ .
RUN dotnet restore ErpAcl.Api/ErpAcl.Api.csproj
WORKDIR /src/ErpAcl.Api
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/ErpAcl.Api
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ErpAcl.Api.dll"]

