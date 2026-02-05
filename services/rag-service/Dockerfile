FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Rag.Api/Rag.Api.csproj src/Rag.Api/
COPY src/Rag.Application/Rag.Application.csproj src/Rag.Application/
COPY src/Rag.Domain/Rag.Domain.csproj src/Rag.Domain/
COPY src/Rag.Infrastructure/Rag.Infrastructure.csproj src/Rag.Infrastructure/

RUN dotnet restore src/Rag.Api/Rag.Api.csproj

COPY src/ .

WORKDIR /src/src/Rag.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8083

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Rag.Api.dll"]

