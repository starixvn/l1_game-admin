using System;
using System.Collections.Generic;
using GamesAdmin.Api.Report;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
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
            { (int)GameId.SeDie, typeof(SeDieReportService) },
            { (int)GameId.MiniSeDie, typeof(MiniSeDieReportService) },
            { (int)GameId.Baccarat, typeof(BaccaratReportService) },
            { (int)GameId.DragonTiger, typeof(DragonTigerReportService) },
            { (int)GameId.KenoProMax, typeof(KenoProMaxReportService) },
            { (int)GameId.KenoMax, typeof(MiniKenoReportService<KenoMaxRoundEntity, KenoMaxOnesEntity, KenoMaxBetEntity>) },
            { (int)GameId.KenoMax2, typeof(MiniKenoReportService<KenoMax2RoundEntity, KenoMax2OnesEntity, KenoMax2BetEntity>) },
            { (int)GameId.KenoMini, typeof(MiniKenoReportService<KenoMiniRoundEntity, KenoMiniOnesEntity, KenoMiniBetEntity>) },
            { (int)GameId.KenoMini2, typeof(MiniKenoReportService<KenoMini2RoundEntity, KenoMini2OnesEntity, KenoMini2BetEntity>) },
            { (int)GameId.KenoEast, typeof(MiniKenoReportService<KenoEastRoundEntity, KenoEastOnesEntity, KenoEastBetEntity>) },
            { (int)GameId.KenoWest, typeof(MiniKenoReportService<KenoWestRoundEntity, KenoWestOnesEntity, KenoWestBetEntity>) },
            { (int)GameId.KenoSouth, typeof(MiniKenoReportService<KenoSouthRoundEntity, KenoSouthOnesEntity, KenoSouthBetEntity>) },
            { (int)GameId.KenoNorth, typeof(MiniKenoReportService<KenoNorthRoundEntity, KenoNorthOnesEntity, KenoNorthBetEntity>) },
            { (int)GameId.Roulette, typeof(RouletteReportService) },
            { (int)GameId.MiniRoulette, typeof(MiniRouletteReportService) },
            { (int)GameId.VipSeDie, typeof(VipSeDieReportService) },
            { (int)GameId.VipFishPrawnCrab, typeof(VipFishPrawnCrabReportService) },
            { (int)GameId.SicBo, typeof(SicBoReportService) },
            { (int)GameId.VipBaccarat, typeof(BaccaratReportService) },
            { (int)GameId.VipSeDieWar, typeof(VipSeDieWarReportService) },
            { (int)GameId.Lottery, typeof(LotteryReportService) },
            { (int)GameId.ThreeCards, typeof(ThreeCardsReportService) },
            { (int)GameId.KenoMatch1, typeof(KenoMatchReportService<KenoMatch1RoundEntity, KenoMatch1OneEntity, KenoMatch1BetEntity>) },
            { (int)GameId.KenoMatch2, typeof(KenoMatchReportService<KenoMatch2RoundEntity, KenoMatch2OneEntity, KenoMatch2BetEntity>) },
            { (int)GameId.KenoPower1, typeof(KenoPowerReportService<KenoPower1RoundEntity, KenoPower1OneEntity, KenoPower1BetEntity>) },
            { (int)GameId.KenoPower2, typeof(KenoPowerReportService<KenoPower2RoundEntity, KenoPower2OneEntity, KenoPower2BetEntity>) },
            { (int)GameId.HiLo, typeof(HiLoReportService) },
            { (int)GameId.BlockchainBaccarat,  typeof(BaccaratReportService) },
            { (int)GameId.MegaKeno, typeof(MiniKenoReportService<MegaKenoRoundEntity, MegaKenoOnesEntity, MegaKenoBetEntity>) },
            { (int)GameId.MegaKeno2, typeof(MiniKenoReportService<MegaKeno2RoundEntity, MegaKeno2OnesEntity, MegaKeno2BetEntity>) },
            { (int)GameId.MiniSeDie2, typeof(MiniSeDieMultiplayReportService<MiniSeDie2RoundEntity, MiniSeDie2BetEntity>) },
            { (int)GameId.MiniSeDie3, typeof(MiniSeDieMultiplayReportService<MiniSeDie3RoundEntity, MiniSeDie3BetEntity>) },
            { (int)GameId.MiniSeDie4, typeof(MiniSeDieMultiplayReportService<MiniSeDie4RoundEntity, MiniSeDie4BetEntity>) },
            { (int)GameId.UnbalancedBigSmall1, typeof(UnbalancedBigSmallReportService<UnbalancedBigSmall1RoundEntity, UnbalancedBigSmall1BetEntity, UnbalancedBigSmall1BetLogEntity>) },
            { (int)GameId.UnbalancedBigSmall2, typeof(UnbalancedBigSmallReportService<UnbalancedBigSmall2RoundEntity, UnbalancedBigSmall2BetEntity, UnbalancedBigSmall2BetLogEntity>) },
            { (int)GameId.UnbalancedBigSmall3, typeof(UnbalancedBigSmallReportService<UnbalancedBigSmall3RoundEntity, UnbalancedBigSmall3BetEntity, UnbalancedBigSmall3BetLogEntity>) },
            { (int)GameId.UnbalancedBigSmall4, typeof(UnbalancedBigSmallReportService<UnbalancedBigSmall4RoundEntity, UnbalancedBigSmall4BetEntity, UnbalancedBigSmall4BetLogEntity>) },
            { (int)GameId.FruitSlot, typeof(FruitSlotReportService) },
            { (int)GameId.FanTan, typeof(FanTanReportService) },
            { (int)GameId.WinThreeCards, typeof(WinThreeCardsReportService) },
            { (int)GameId.DragonTigerFight, typeof(DragonTigerFightReportService) },
            { (int)GameId.ColorGame, typeof(ColorGameReportService) },
            { (int)GameId.BandarQiuQiu, typeof(BandarQiuQiuReportService) },
            { (int)GameId.BacBall1, typeof(BacBall1ReportService) },
            { (int)GameId.BacBall2, typeof(BacBall2ReportService) },
            { (int)GameId.LuckyNext1, typeof(NextBallReportService<LuckyNext1RoundEntity, LuckyNext1BetEntity, LuckyNext1BetLogEntity>) },
            { (int)GameId.LuckyNext2, typeof(NextBallReportService<LuckyNext2RoundEntity, LuckyNext2BetEntity, LuckyNext2BetLogEntity>) },
            { (int)GameId.FootballDice, typeof(FootballDiceReportService) },
            { (int)GameId.FootballSlot, typeof(FootballSlotReportService) },
        };

        private readonly IServiceProvider serviceProvider;

        public ReportServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IGameReportService GetService(int game)
            => (IGameReportService)serviceProvider.GetRequiredService(services[game]);

        public IBetHistoryReportService GetBetHistoryService(int game)
            => (IBetHistoryReportService)serviceProvider.GetRequiredService(services[game]);
    }
}