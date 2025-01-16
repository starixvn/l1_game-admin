FROM hub.lumigame.com/base/dotnet/core/sdk:3.1-5 AS builder
WORKDIR /src

COPY ./GamesAdmin/src/GamesAdmin.Core ./GamesAdmin.Core
COPY ./GamesAdmin/src/GamesAdmin.Database ./GamesAdmin.Database
COPY ./GamesAdmin/src/GamesAdmin.Api/GamesAdmin.Api.csproj ./GamesAdmin.Api/GamesAdmin.Api.csproj

WORKDIR /src/GamesAdmin.Api
RUN dotnet restore -v diag

COPY ./GamesAdmin/src/GamesAdmin.Api .

RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM hub.lumigame.com/base/dotnet/core/aspnet:3.1.32-alpine AS base

RUN apk update && apk upgrade && apk add --no-cache curl

RUN adduser admin_api_user --disabled-password
USER admin_api_user 
WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "GamesAdmin.Api.dll"]