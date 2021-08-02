using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.GameServers
{
    public interface ISlotBoardApiFactory
    {
        ISlotBoardApi SetBoardApi(Game game);
    }

    public class SlotBoardApiFactory : ISlotBoardApiFactory
    {
        private readonly Dictionary<string, Type> slotServices = new Dictionary<string, Type>
        {
            { Game.FourGuardiansValue, typeof(IFourGuardiansBoardApi) },
            { Game.VietnameseHeroesValue, typeof(IVietnameseHeroesBoardApi) },
        };

        private readonly Type defaultSlotBoardApi = typeof(ISlotBoardApi);

        private readonly IServiceProvider serviceProvider;

        public SlotBoardApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ISlotBoardApi SetBoardApi(Game game)
        => slotServices.ContainsKey(game.Value)
            ? (ISlotBoardApi)serviceProvider.GetService(slotServices[game.Value])
            : (ISlotBoardApi)serviceProvider.GetService(defaultSlotBoardApi);
    }
}