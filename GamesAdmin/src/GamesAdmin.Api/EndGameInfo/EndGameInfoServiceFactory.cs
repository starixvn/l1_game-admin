using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.EndGameInfo
{
    public interface IEndGameInfoServiceFactory
    {
        IEndGameInfoService GetService(GameId game);
    }

    public class EndGameInfoServiceFactory : IEndGameInfoServiceFactory
    {
        private readonly Dictionary<GameId, Type> services = new Dictionary<GameId, Type>
        {
            { GameId.BigSmall, typeof(BigSmallEndGameInfoService) },
        };

        private readonly IServiceProvider serviceProvider;

        public EndGameInfoServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEndGameInfoService GetService(GameId game)
        => (IEndGameInfoService) serviceProvider.GetRequiredService(services[game]);
    }
}
