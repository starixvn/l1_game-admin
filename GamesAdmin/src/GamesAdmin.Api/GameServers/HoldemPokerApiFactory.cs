using GamesAdmin.Core.Enumeration;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Api.GameServers
{
    public interface IHoldemPokerApiFactory
    {
        IHoldemPokerApi SetResultApi(Game game);
    }

    public class HoldemPokerApiFactory : IHoldemPokerApiFactory
    {
        private readonly Dictionary<string, Type> pokerServices = new Dictionary<string, Type>
        {
            { Game.CasinoHoldemPokerValue, typeof(ICasinoHoldemPokerApi) },
            { Game.TexasHoldemPokerValue, typeof(ITexasHoldemPokerApi) },
        };

        private readonly Type defaulHoldemPokerApi = typeof(IHoldemPokerApi);

        private readonly IServiceProvider serviceProvider;

        public HoldemPokerApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IHoldemPokerApi SetResultApi(Game game)
            => pokerServices.ContainsKey(game.Value)
                ? (IHoldemPokerApi)serviceProvider.GetService(pokerServices[game.Value])
                : (IHoldemPokerApi)serviceProvider.GetService(defaulHoldemPokerApi);
    }
}