using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
            { GameId.Roulette, typeof(RouletteEndGameInfoService) },
            { GameId.VipSeDie, typeof(VipSeDieEndGameInfoService) },
            { GameId.SpecialSeDie, typeof(SpecialSeDieEndGameInfoService) },
            { GameId.Blackjack, typeof(BlackjackEndGameInfoService) },
            { GameId.VipFishPrawnCrab, typeof(VipFishPrawnCrabEndGameInfoService) },
            { GameId.SicBo, typeof(SicBoEndGameInfoService) },
            { GameId.VipBaccarat, typeof(VipBaccaratEndGameInfoService) },
            { GameId.VipSeDieWar, typeof(VipSeDieWarEndGameInfoService) },
            { GameId.FourGuardians, typeof(FourGuardiansEndGameInfoService) },
            { GameId.Lottery, typeof(LotteryEndGameInfoService) },
            { GameId.ThreeCards, typeof(ThreeCardsEndGameInfoService) },
            { GameId.HeroesSlot, typeof(HeroesSlotEndGameInfoService) },
            { GameId.BitcoinMaster, typeof(BitcoinMasterEndGameInfoService) },
            { GameId.KenoMatch1, typeof(KenoMatchEndGameInfoService<KenoMatch1RoundEntity, KenoMatch1OneEntity>) },
            { GameId.KenoMatch2, typeof(KenoMatchEndGameInfoService<KenoMatch2RoundEntity, KenoMatch2OneEntity>) },
            { GameId.KenoPower1, typeof(KenoPowerEndGameInfoService<KenoPower1RoundEntity, KenoPower1OneEntity>) },
            { GameId.KenoPower2, typeof(KenoPowerEndGameInfoService<KenoPower2RoundEntity, KenoPower2OneEntity>) },
            { GameId.CasinoHoldemPoker, typeof(CasinoHoldemPokerEndGameInfoService) },
            { GameId.HiLo, typeof(HiLoEndGameInfoService) },
            { GameId.TexasHoldemPoker, typeof(TexasHoldemPokerEndGameInfoService) },
            { GameId.NightClub, typeof(NightClubEndGameInfoService) },
        };

        private readonly IServiceProvider serviceProvider;

        public EndGameInfoServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IEndGameInfoService GetService(GameId game)
        => (IEndGameInfoService)serviceProvider.GetRequiredService(services[game]);
    }
}