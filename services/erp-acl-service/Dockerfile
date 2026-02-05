FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/ErpAcl.Api/ErpAcl.Api.csproj src/ErpAcl.Api/
COPY src/ErpAcl.Application/ErpAcl.Application.csproj src/ErpAcl.Application/
COPY src/ErpAcl.Domain/ErpAcl.Domain.csproj src/ErpAcl.Domain/
COPY src/ErpAcl.Infrastructure/ErpAcl.Infrastructure.csproj src/ErpAcl.Infrastructure/

RUN dotnet restore src/ErpAcl.Api/ErpAcl.Api.csproj

COPY src/ .

WORKDIR /src/src/ErpAcl.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ErpAcl.Api.dll"]

