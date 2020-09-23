FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

ARG DEPLOY_ENV=DEV
RUN echo $DEPLOY_ENV

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
RUN dotnet publish -c $DEPLOY_ENV -r linux-x64 --self-contained false -o /app 

FROM build AS publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "GamesAdmin.Site.dll"]