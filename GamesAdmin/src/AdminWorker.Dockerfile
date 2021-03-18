FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:3.1 AS builder
WORKDIR /src

COPY ./GamesAdmin/src/GamesAdmin.Core ./GamesAdmin.Core
COPY ./GamesAdmin/src/GamesAdmin.Database ./GamesAdmin.Database
COPY ./GamesAdmin/src/GamesAdmin.Workers/GamesAdmin.Workers.csproj ./GamesAdmin.Workers/GamesAdmin.Workers.csproj

WORKDIR /src/GamesAdmin.Workers
RUN dotnet restore -v diag

COPY ./GamesAdmin/src/GamesAdmin.Workers .

RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app
COPY --from=builder /app .

RUN    apt-get update \
    && apt-get install openssl \
    && apt-get install ca-certificates

RUN sed -i "s|TLSv1.2|TLSv1.0|g" /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "GamesAdmin.Workers.dll"]