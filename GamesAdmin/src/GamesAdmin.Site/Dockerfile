FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

ENV ASPNETCORE_URLS=https://+;http://+;

COPY ~/build/game-admin-site .

ENTRYPOINT ["dotnet", "watch", "run", "--no-restore", "--urls", "https://0.0.0.0:5000", "GamesAdmin.Site.dll"]