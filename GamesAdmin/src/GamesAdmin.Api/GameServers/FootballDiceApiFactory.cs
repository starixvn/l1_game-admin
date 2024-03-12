using System;

namespace GamesAdmin.Api.GameServers
{
    public interface IFootballDiceApiFactory
    {
        IFootballDiceApi SetResultApi();
    }

    public class FootballDiceApiFactory : IFootballDiceApiFactory
    {
        private readonly IServiceProvider serviceProvider;

        public FootballDiceApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IFootballDiceApi SetResultApi()
            => (IFootballDiceApi)serviceProvider.GetService(typeof(IFootballDiceApi));
    }
}
