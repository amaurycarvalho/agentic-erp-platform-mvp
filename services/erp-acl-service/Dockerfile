FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/ .

RUN dotnet restore ErpAcl.Api/ErpAcl.Api.csproj

WORKDIR /src/ErpAcl.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ErpAcl.Api.dll"]

