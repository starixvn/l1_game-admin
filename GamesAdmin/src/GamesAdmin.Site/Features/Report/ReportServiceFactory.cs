using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site.Features.Report
{
    public interface IReportServiceFactory
    {
        IGameReportService GetService(GameId game);
        IBetHistoryReportService GetBetHistoryService(GameId game);
    }

    public class ReportServiceFactory : IReportServiceFactory
    {
        private readonly Dictionary<GameId, Type> services = new Dictionary<GameId, Type>
        {
            { GameId.FishPrawnCrab, typeof(FishPrawnCrabReportService) },
            { GameId.SeDie, typeof(SedieReportService) },
            { GameId.MiniSeDie, typeof(MiniSedieReportService) },
            { GameId.DragonTiger, typeof(DragonTigerReportService) },
            { GameId.KenoProMax, typeof(MiniKenoReportService) },
            { GameId.KenoMax, typeof(MiniKenoReportService) },
            { GameId.KenoMax2, typeof(MiniKenoReportService) },
            { GameId.KenoMini, typeof(MiniKenoReportService) },
            { GameId.KenoMini2, typeof(MiniKenoReportService) },
            { GameId.KenoEast, typeof(MiniKenoReportService) },
            { GameId.KenoWest, typeof(MiniKenoReportService) },
            { GameId.KenoSouth, typeof(MiniKenoReportService) },
            { GameId.KenoNorth, typeof(MiniKenoReportService) },
            { GameId.Roulette, typeof(RouletteReportService) },
            { GameId.VipSeDie, typeof(VipSeDieReportService) },
            { GameId.VipFishPrawnCrab, typeof(VipFishPrawnCrabReportService) },
            { GameId.SicBo, typeof(SicBoReportService) },
            { GameId.VipSeDieWar, typeof(VipSeDieWarReportService) },
            { GameId.Lottery, typeof(LotteryReportService) },
            { GameId.ThreeCards, typeof(ThreeCardsReportService) },
            { GameId.KenoMatch1, typeof(KenoMatchReportService) },
            { GameId.KenoMatch2, typeof(KenoMatchReportService) },
            { GameId.KenoPower1, typeof(KenoPowerReportService) },
            { GameId.KenoPower2, typeof(KenoPowerReportService) },
            { GameId.HiLo, typeof(HiLoReportService) },
            { GameId.Baccarat, typeof(BaccaratReportService) },
            { GameId.VipBaccarat, typeof(BaccaratReportService) },
            { GameId.BlockchainBaccarat, typeof(BaccaratReportService) },
            { GameId.MegaKeno, typeof(MiniKenoReportService) },
            { GameId.MegaKeno2, typeof(MiniKenoReportService) },
            { GameId.MiniSeDie2, typeof(SedieReportService) },
            { GameId.MiniSeDie3, typeof(SedieReportService) },
            { GameId.MiniSeDie4, typeof(SedieReportService) },
            { GameId.UnbalancedBigSmall1, typeof(UnbalancedBigSmallReportService) },
            { GameId.UnbalancedBigSmall2, typeof(UnbalancedBigSmallReportService) },
            { GameId.UnbalancedBigSmall3, typeof(UnbalancedBigSmallReportService) },
            { GameId.UnbalancedBigSmall4, typeof(UnbalancedBigSmallReportService) },
            { GameId.FruitSlot, typeof(FruitSlotReportService) },
        };

        private readonly IServiceProvider serviceProvider;

        public ReportServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IBetHistoryReportService GetBetHistoryService(GameId game)
            => (IBetHistoryReportService)serviceProvider.GetRequiredService(services[game]);

        public IGameReportService GetService(GameId game)
            => (IGameReportService)serviceProvider.GetRequiredService(services[game]);
    }
}