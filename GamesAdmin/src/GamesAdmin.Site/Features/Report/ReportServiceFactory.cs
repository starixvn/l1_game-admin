using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site.Features.Report
{
    public interface IReportServiceFactory
    {
        IGameReportService GetService(int game);

        IBetHistoryReportService GetBetHistoryService(int game);
    }

    public class ReportServiceFactory : IReportServiceFactory
    {
        private readonly Dictionary<int, Type> services = new Dictionary<int, Type>
        {
            { (int)GameId.FishPrawnCrab, typeof(FishPrawnCrabReportService) },
            { (int)GameId.SeDie, typeof(SedieReportService) },
            { (int)GameId.MiniSeDie, typeof(MiniSedieReportService) },
            { (int)GameId.DragonTiger, typeof(DragonTigerReportService) },
            { (int)GameId.KenoProMax, typeof(MiniKenoReportService) },
            { (int)GameId.KenoMax, typeof(MiniKenoReportService) },
            { (int)GameId.KenoMax2, typeof(MiniKenoReportService) },
            { (int)GameId.KenoMini, typeof(MiniKenoReportService) },
            { (int)GameId.KenoMini2, typeof(MiniKenoReportService) },
            { (int)GameId.KenoEast, typeof(MiniKenoReportService) },
            { (int)GameId.KenoWest, typeof(MiniKenoReportService) },
            { (int)GameId.KenoSouth, typeof(MiniKenoReportService) },
            { (int)GameId.KenoNorth, typeof(MiniKenoReportService) },
            { (int)GameId.Roulette, typeof(RouletteReportService) },
            { (int)GameId.MiniRoulette, typeof(MiniRouletteReportService) },
            { (int)GameId.VipSeDie, typeof(VipSeDieReportService) },
            { (int)GameId.VipFishPrawnCrab, typeof(VipFishPrawnCrabReportService) },
            { (int)GameId.SicBo, typeof(SicBoReportService) },
            { (int)GameId.VipSeDieWar, typeof(VipSeDieWarReportService) },
            { (int)GameId.Lottery, typeof(LotteryReportService) },
            { (int)GameId.ThreeCards, typeof(ThreeCardsReportService) },
            { (int)GameId.KenoMatch1, typeof(KenoMatchReportService) },
            { (int)GameId.KenoMatch2, typeof(KenoMatchReportService) },
            { (int)GameId.KenoPower1, typeof(KenoPowerReportService) },
            { (int)GameId.KenoPower2, typeof(KenoPowerReportService) },
            { (int)GameId.HiLo, typeof(HiLoReportService) },
            { (int)GameId.Baccarat, typeof(BaccaratReportService) },
            { (int)GameId.VipBaccarat, typeof(BaccaratReportService) },
            { (int)GameId.BlockchainBaccarat, typeof(BaccaratReportService) },
            { (int)GameId.MegaKeno, typeof(MiniKenoReportService) },
            { (int)GameId.MegaKeno2, typeof(MiniKenoReportService) },
            { (int)GameId.MiniSeDie2, typeof(SedieReportService) },
            { (int)GameId.MiniSeDie3, typeof(SedieReportService) },
            { (int)GameId.MiniSeDie4, typeof(SedieReportService) },
            { (int)GameId.UnbalancedBigSmall1, typeof(UnbalancedBigSmallReportService) },
            { (int)GameId.UnbalancedBigSmall2, typeof(UnbalancedBigSmallReportService) },
            { (int)GameId.UnbalancedBigSmall3, typeof(UnbalancedBigSmallReportService) },
            { (int)GameId.UnbalancedBigSmall4, typeof(UnbalancedBigSmallReportService) },
            { (int)GameId.FruitSlot, typeof(SlotRNGReportService) },
            { (int)GameId.FanTan, typeof(FanTanReportService) },
            { (int)GameId.WinThreeCards, typeof(WinThreeCardsReportService) },
            { (int)GameId.DragonTigerFight, typeof(DragonTigerFightReportService) },
            { (int)GameId.ColorGame, typeof(ColorGameReportService) },
            { (int)GameId.BandarQiuQiu, typeof(BandarQiuQiuReportService) },
            { (int)GameId.BacBall1, typeof(BacBall1ReportService) },
            { (int)GameId.BacBall2, typeof(BacBall2ReportService) },
            { (int)GameId.LuckyNext1, typeof(NextBallReportService) },
            { (int)GameId.LuckyNext2, typeof(NextBallReportService) },
            { (int)GameId.FootballDice, typeof(FootballDiceReportService) },
            { (int)GameId.FootballSlot, typeof(SlotRNGReportService) },
        };

        private readonly IServiceProvider serviceProvider;

        public ReportServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IBetHistoryReportService GetBetHistoryService(int game)
            => (IBetHistoryReportService)serviceProvider.GetRequiredService(services[game]);

        public IGameReportService GetService(int game)
            => (IGameReportService)serviceProvider.GetRequiredService(services[game]);
    }
}