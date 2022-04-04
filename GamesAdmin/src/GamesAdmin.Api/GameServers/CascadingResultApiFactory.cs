using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.GameServers
{
    public interface ICascadingResultApiFactory
    {
        ICascadingSlotResultApi SetResultApi(Game game);
    }

    public class CascadingResultApiFactory : ICascadingResultApiFactory
    {
        private readonly Dictionary<string, Type> slotServices = new Dictionary<string, Type>
        {
            { Game.TreasureIslandValue, typeof(ITreasureIslandResultApi) },
        };

        private readonly Type defaultSlotResultApi = typeof(ICascadingSlotResultApi);

        private readonly IServiceProvider serviceProvider;

        public CascadingResultApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICascadingSlotResultApi SetResultApi(Game game)
        => slotServices.ContainsKey(game.Value)
            ? (ICascadingSlotResultApi)serviceProvider.GetService(slotServices[game.Value])
            : (ICascadingSlotResultApi)serviceProvider.GetService(defaultSlotResultApi);
    }
}