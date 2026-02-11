FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY src/Rag.Api/Rag.Api.csproj src/Rag.Api/
COPY src/Rag.Application/Rag.Application.csproj src/Rag.Application/
COPY src/Rag.Domain/Rag.Domain.csproj src/Rag.Domain/
COPY src/Rag.Infrastructure/Rag.Infrastructure.csproj src/Rag.Infrastructure/

RUN dotnet restore src/Rag.Api/Rag.Api.csproj

COPY src/ src/

RUN dotnet publish src/Rag.Api/Rag.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
RUN useradd -m appuser

EXPOSE 8083

COPY --from=build /app/publish .
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "Rag.Api.dll"]
