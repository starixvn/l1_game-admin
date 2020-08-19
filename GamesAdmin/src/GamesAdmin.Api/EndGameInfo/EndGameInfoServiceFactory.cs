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
            { GameId.BigSmallTurbo, typeof(BigSmallTurboEndGameInfoService) },
            { GameId.OddEven, typeof(OddEvenEndGameInfoService) },
            { GameId.OddEvenTurbo, typeof(OddEvenTurboEndGameInfoService) },
            { GameId.FishPrawnCrab, typeof(FishPrawnCrabEndGameInfoService) },
            { GameId.SeDie, typeof(SeDieEndGameInfoService) },
            { GameId.MiniSeDie, typeof(MiniSeDieEndGameInfoService) },
            { GameId.Baccarat, typeof(BaccaratEndGameInfoService) },
            { GameId.DragonTiger, typeof(DragonTigerEndGameInfoService) },
            { GameId.KenoProMax, typeof(KenoProMaxEndGameInfoService) },
            { GameId.KenoProViet, typeof(KenoProVietEndGameInfoService) },
            { GameId.KenoMax, typeof(KenoMaxEndGameInfoService) },
            { GameId.KenoMax2, typeof(KenoMax2EndGameInfoService) },
            { GameId.KenoMini, typeof(KenoMiniEndGameInfoService) },
            { GameId.KenoMini2, typeof(KenoMini2EndGameInfoService) },
            { GameId.KenoEast, typeof(KenoEastEndGameInfoService) },
            { GameId.KenoWest, typeof(KenoWestEndGameInfoService) },
            { GameId.KenoSouth, typeof(KenoSouthEndGameInfoService) },
            { GameId.KenoNorth, typeof(KenoNorthEndGameInfoService) },
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
