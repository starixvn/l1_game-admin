FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /src

ARG DEPLOY_ENV=DEV
RUN echo $DEPLOY_ENV

COPY ./GamesAdmin/src/GamesAdmin.Core ./GamesAdmin.Core
COPY ./GamesAdmin/src/GamesAdmin.Database ./GamesAdmin.Database
COPY ./GamesAdmin/src/GamesAdmin.Api/GamesAdmin.Api.csproj ./GamesAdmin.Api/GamesAdmin.Api.csproj

WORKDIR /src/GamesAdmin.Api
RUN dotnet restore -v diag

COPY ./GamesAdmin/src/GamesAdmin.Api .
RUN dotnet publish -c $DEPLOY_ENV -r linux-x64 --self-contained false -o /app 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "GamesAdmin.Api.dll"]