FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:3.1 AS builder
WORKDIR /src

COPY ./GamesAdmin/src/GamesAdmin.Core ./GamesAdmin.Core
COPY ./GamesAdmin/src/GamesAdmin.Database ./GamesAdmin.Database
COPY ./GamesAdmin/src/GamesAdmin.Api/GamesAdmin.Api.csproj ./GamesAdmin.Api/GamesAdmin.Api.csproj

WORKDIR /src/GamesAdmin.Api
RUN dotnet restore -v diag

COPY ./GamesAdmin/src/GamesAdmin.Api .

RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "GamesAdmin.Api.dll"]