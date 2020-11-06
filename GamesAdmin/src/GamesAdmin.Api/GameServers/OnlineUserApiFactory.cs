using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.GameServers
{
    public interface IOnlineUserApiFactory
    {
        IOnlineUserApi GetOnlineUserApi(Game game);
    }

    public class OnlineUserApiFactory : IOnlineUserApiFactory
    {
        private readonly Dictionary<string, Type> onlineUserServices = new Dictionary<string, Type>
        {
            { Game.SeDieValue, typeof(ISedieOnlineUserApi) },
            { Game.VipSeDieValue, typeof(IVipSedieOnlineUserApi) },
            { Game.SpecialSeDieValue, typeof(ISpecialSedieOnlineUserApi) },
            { Game.BlackJackValue, typeof(IBlackjackOnlineUserApi) },
            { GameType.VipFishPrawnCrabValue, typeof(IVipFishPrawnCrabOnlineUserApi) },
            { GameType.SicBoValue, typeof(ISicBoOnlineUserApi) },
            { GameType.VipBaccaratValue, typeof(IVipBaccaratOnlineUserApi) },
            { GameType.VipSeDieWarValue, typeof(IVipSedieWarOnlineUserApi) },
        };

        private readonly Type defaultOnlineUserApi = typeof(IMainOnlineUserApi);

        private readonly IServiceProvider serviceProvider;

        public OnlineUserApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        IOnlineUserApi IOnlineUserApiFactory.GetOnlineUserApi(Game game)
        => onlineUserServices.ContainsKey(game.Value)
            ? (IOnlineUserApi)serviceProvider.GetService(onlineUserServices[game.Value])
            : (IOnlineUserApi)serviceProvider.GetService(defaultOnlineUserApi);
    }
}
