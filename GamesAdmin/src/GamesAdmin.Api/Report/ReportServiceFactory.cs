using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Keno;
using GamesAdmin.Database.Entities.KenoProMax;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public interface IReportServiceFactory
    {
        IGameReportService GetService(GameId game);
    }

    public class ReportServiceFactory : IReportServiceFactory
    {
        private readonly Dictionary<GameId, Type> services = new Dictionary<GameId, Type>
        {
            //{ GameId.BigSmall, typeof(BigSmallEndGameInfoService) },
            //{ GameId.BigSmallTurbo, typeof(BigSmallTurboEndGameInfoService) },
            //{ GameId.OddEven, typeof(OddEvenEndGameInfoService) },
            //{ GameId.OddEvenTurbo, typeof(OddEvenTurboEndGameInfoService) },
            { GameId.FishPrawnCrab, typeof(FishPrawnCrabReportService) },
            { GameId.SeDie, typeof(SeDieReportService) },
            { GameId.MiniSeDie, typeof(MiniSeDieReportService) },
            { GameId.Baccarat, typeof(BaccaratReportService) },
            { GameId.DragonTiger, typeof(DragonTigerReportService) },
            { GameId.KenoProMax, typeof(MiniKenoReportService<KenoProMaxRoundEntity, KenoProMaxBetEntity>) },
            { GameId.KenoMax, typeof(MiniKenoReportService<KenoMaxRoundEntity, KenoMaxOnesEntity>) },
            { GameId.KenoMax2, typeof(MiniKenoReportService<KenoMax2RoundEntity, KenoMax2OnesEntity>) },
            { GameId.KenoMini, typeof(MiniKenoReportService<KenoMiniRoundEntity, KenoMiniOnesEntity>) },
            { GameId.KenoMini2, typeof(MiniKenoReportService<KenoMini2RoundEntity, KenoMini2OnesEntity>) },
            { GameId.KenoEast, typeof(MiniKenoReportService<KenoEastRoundEntity, KenoEastOnesEntity>) },
            { GameId.KenoWest, typeof(MiniKenoReportService<KenoWestRoundEntity, KenoWestOnesEntity>) },
            { GameId.KenoSouth, typeof(MiniKenoReportService<KenoSouthRoundEntity, KenoSouthOnesEntity>) },
            { GameId.KenoNorth, typeof(MiniKenoReportService<KenoNorthRoundEntity, KenoNorthOnesEntity>) },
            { GameId.Roulette, typeof(RouletteReportService) },
            { GameId.VipSeDie, typeof(VipSeDieReportService) },
            //{ GameId.SpecialSeDie, typeof(SpecialSeDieEndGameInfoService) },
            //{ GameId.Blackjack, typeof(BlackjackEndGameInfoService) },
            { GameId.VipFishPrawnCrab, typeof(VipFishPrawnCrabReportService) },
            { GameId.SicBo, typeof(SicBoReportService) },
            { GameId.VipBaccarat, typeof(VipBaccaratReportService) },
            { GameId.VipSeDieWar, typeof(VipSeDieWarReportService) },
            //{ GameId.FourGuardians, typeof(FourGuardiansEndGameInfoService) },
            { GameId.Lottery, typeof(LotteryReportService) },
            { GameId.ThreeCards, typeof(ThreeCardsReportService) },
            //{ GameId.HeroesSlot, typeof(HeroesSlotEndGameInfoService) },
            //{ GameId.BitcoinMaster, typeof(BitcoinMasterEndGameInfoService) },
            { GameId.KenoMatch1, typeof(KenoMatchReportService<KenoMatch1RoundEntity, KenoMatch1OneEntity>) },
            { GameId.KenoMatch2, typeof(KenoMatchReportService<KenoMatch2RoundEntity, KenoMatch2OneEntity>) },
            { GameId.KenoPower1, typeof(KenoPowerReportService<KenoPower1RoundEntity, KenoPower1OneEntity>) },
            { GameId.KenoPower2, typeof(KenoPowerReportService<KenoPower2RoundEntity, KenoPower2OneEntity>) },
        };

        private readonly IServiceProvider serviceProvider;

        public ReportServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IGameReportService GetService(GameId game)
        => (IGameReportService)serviceProvider.GetRequiredService(services[game]);
    }
}
