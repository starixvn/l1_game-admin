FROM git3.nexdev.net:5050/lumi-ops/docker-images/dotnet/core/sdk:3.1 AS builder
WORKDIR /src

COPY ./GamesAdmin/src/GamesAdmin.Core ./GamesAdmin.Core
COPY ./GamesAdmin/src/GamesAdmin.Database ./GamesAdmin.Database
COPY ./GamesAdmin/src/GamesAdmin.Api/GamesAdmin.Api.csproj ./GamesAdmin.Api/GamesAdmin.Api.csproj

WORKDIR /src/GamesAdmin.Api
RUN dotnet restore -v diag

COPY ./GamesAdmin/src/GamesAdmin.Api .

ARG CACHEBUST=1 
ARG DEPLOY_ENV=DEV
RUN echo $DEPLOY_ENV $CACHEBUST

RUN dotnet publish -c $DEPLOY_ENV -r linux-x64 --self-contained false -o /app 

FROM git3.nexdev.net:5050/lumi-ops/docker-images/dotnet/core/aspnet:3.1 AS base

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "GamesAdmin.Api.dll"]