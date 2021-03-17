FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:3.1 AS builder
WORKDIR /src

RUN apt-get update
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs

COPY ./GamesAdmin/src/game-admin-client ./game-admin-client
COPY ./GamesAdmin/src/GamesAdmin.Core ./GamesAdmin.Core
COPY ./GamesAdmin/src/GamesAdmin.Database ./GamesAdmin.Database
COPY ./GamesAdmin/src/GamesAdmin.Site/GamesAdmin.Site.csproj ./GamesAdmin.Site/GamesAdmin.Site.csproj

WORKDIR /src/GamesAdmin.Site
RUN dotnet restore -v diag

COPY ./GamesAdmin/src/GamesAdmin.Site .
RUN npm run export --prefix ../game-admin-client

RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "GamesAdmin.Site.dll"]
