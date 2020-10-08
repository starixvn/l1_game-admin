using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.GameServers
{
    public interface IOnlineUserApiFactory
    {
        IOnlineUserApi GetOnlineUserApi(GameType game);
    }

    public class OnlineUserApiFactory : IOnlineUserApiFactory
    {
        private readonly Dictionary<string, Type> onlineUserServices = new Dictionary<string, Type>
        {
            { GameType.SeDieValue, typeof(ISedieOnlineUserApi) }
        };

        private readonly Type defaultOnlineUserApi = typeof(IMainOnlineUserApi);

        private readonly IServiceProvider serviceProvider;

        public OnlineUserApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        IOnlineUserApi IOnlineUserApiFactory.GetOnlineUserApi(GameType game)
        => onlineUserServices.ContainsKey(game.Value)
            ? (IOnlineUserApi)serviceProvider.GetService(onlineUserServices[game.Value])
            : (IOnlineUserApi)serviceProvider.GetService(defaultOnlineUserApi);
    }
}
