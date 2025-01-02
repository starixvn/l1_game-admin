using System;
using System.Collections.Generic;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;

namespace GamesAdmin.Api.VoidGame
{
    public static class TicketCollectionHelper
    {
        private static Dictionary<GameId, GameTicketCollection> gameCollections = new Dictionary<GameId, GameTicketCollection>
        {
            { GameId.BigSmall, new GameTicketCollection("taixiu_ones", "taixiu_cuocs") },
            { GameId.SeDie, new GameTicketCollection(string.Empty, "xocxoc_cuocs") },
            { GameId.FishPrawnCrab, new GameTicketCollection( string.Empty, "baucua_cuocs") },
            { GameId.BigSmallTurbo, new GameTicketCollection("txturbo_ones", "txturbo_cuocs") },
            { GameId.OddEven, new GameTicketCollection( "chanle_ones", "chanle_cuocs") },
            { GameId.OddEvenTurbo, new GameTicketCollection( "chanleturbo_ones", "chanleturbo_cuocs") },
            { GameId.DragonTiger, new GameTicketCollection( string.Empty, "minidragontiger_cuocs") },
            { GameId.KenoProMax, new GameTicketCollection( string.Empty, "kenopromax_cuocs") },
            { GameId.MiniSeDie, new GameTicketCollection( string.Empty, "minisedie_cuocs") },
            { GameId.Baccarat, new GameTicketCollection( string.Empty, "baccarat_cuocs") },
            { GameId.KenoMax, new GameTicketCollection( "kenomax_ones", "kenomax_cuocs") },
            { GameId.KenoMax2, new GameTicketCollection( "kenomax2_ones", "kenomax2_cuocs") },
            { GameId.Roulette, new GameTicketCollection( "roulette_ones", "roulette_cuocs") },
            { GameId.VipSeDie, new GameTicketCollection( string.Empty, "vipsedie_cuocs") },
            { GameId.Blackjack, new GameTicketCollection( string.Empty, "blackjack_cuocs") },
            { GameId.SpecialSeDie, new GameTicketCollection( "specialsedie_ones", "specialsedie_cuocs") },
            { GameId.SicBo, new GameTicketCollection( "sicbo_ones", "sicbo_cuocs") },
            { GameId.VipFishPrawnCrab, new GameTicketCollection( string.Empty, "vipfishprawncrab_cuocs") },
            { GameId.VipBaccarat, new GameTicketCollection( string.Empty, "vipbaccarat_cuocs") },
            { GameId.VipSeDieWar, new GameTicketCollection( "vipsediewar_ones", "vipsediewar_cuocs") },
            { GameId.ThreeCards, new GameTicketCollection( string.Empty, "threecards_cuocs") },
            { GameId.Lottery, new GameTicketCollection( "lottery_ones", "lottery_cuocs") },
            { GameId.BitcoinMaster, new GameTicketCollection( "bitcoinmaster_bets", "bitcoinmaster_betlogs") },
            { GameId.CasinoHoldemPoker, new GameTicketCollection( "casinoholdempoker_phiens", "casinoholdempoker_cuocs") },
            { GameId.HiLo, new GameTicketCollection( string.Empty, "hilo_betlogs") },
            { GameId.TexasHoldemPoker, new GameTicketCollection( "texasholdempoker_phiens", "texasholdempoker_cuocs") },
            { GameId.BlockchainBaccarat, new GameTicketCollection( string.Empty, "blockchainbaccarat_bets") },
            { GameId.MiniSeDie2, new GameTicketCollection( string.Empty, "minisedie2_bets") },
            { GameId.MiniSeDie3, new GameTicketCollection( string.Empty, "minisedie3_bets") },
            { GameId.MiniSeDie4, new GameTicketCollection( string.Empty, "minisedie4_bets") },
            { GameId.UnbalancedBigSmall1, new GameTicketCollection("unbalancedbigsmall1_bets", "unbalancedbigsmall1_betlogs") },
            { GameId.UnbalancedBigSmall2, new GameTicketCollection("unbalancedbigsmall2_bets", "unbalancedbigsmall2_betlogs") },
            { GameId.UnbalancedBigSmall3, new GameTicketCollection("unbalancedbigsmall3_bets", "unbalancedbigsmall3_betlogs") },
            { GameId.UnbalancedBigSmall4, new GameTicketCollection("unbalancedbigsmall4_bets", "unbalancedbigsmall4_betlogs") },
            { GameId.FruitSlot, new GameTicketCollection("fruitslot_bets", "fruitslot_betlogs") },
            { GameId.FanTan, new GameTicketCollection("fantan_bets", "fantan_betlogs") },
            { GameId.WinThreeCards, new GameTicketCollection("winthreecards_bets", "winthreecards_betlogs") },
            { GameId.DragonTigerFight, new GameTicketCollection("dragontigerfight_bets", "dragontigerfight_betlogs") },
            { GameId.ColorGame, new GameTicketCollection("colorgame_bets", "colorgame_betlogs") },
            { GameId.BandarQiuQiu, new GameTicketCollection("bandarqiuqiu_bets", "bandarqiuqiu_betlogs") },
            { GameId.FootballDice, new GameTicketCollection("footballdice_bets", "footballdice_betlogs") },
            { GameId.FootballSlot, new GameTicketCollection("footballslot_bets", "footballslot_betlogs") },
            { GameId.FootballCoinToss, new GameTicketCollection("footballcointoss_bets", "footballcointoss_betlogs") },
            { GameId.CoinToss, new GameTicketCollection("cointoss_bets", "cointoss_betlogs") },

            //xD
            { GameId.BacBall1, new GameTicketCollection("bacball1_bets", "bacball1_betlogs") },
            { GameId.BacBall2, new GameTicketCollection("bacball2_bets", "bacball2_betlogs") },
            { GameId.LuckyNext1, new GameTicketCollection("luckynext1_bets", "luckynext1_betlogs") },
            { GameId.LuckyNext2, new GameTicketCollection("luckynext2_bets", "luckynext2_betlogs") },
            { GameId.SeBall1, new GameTicketCollection("seball1_bets", "seball1_betlogs") },
            { GameId.LuckyThree, new GameTicketCollection("luckythree_bets", "luckythree_betlogs") },
            { GameId.SicBall, new GameTicketCollection("sicball_bets", "sicball_betlogs") },
            { GameId.BaccaratNumber, new GameTicketCollection("baccaratnumber_bets", "baccaratnumber_betlogs") },
            { GameId.SpeedBaccaratNumber, new GameTicketCollection("speedbaccaratnumber_bets", "speedbaccaratnumber_betlogs") },
            { GameId.VIPBaccaratNumber, new GameTicketCollection("vipbaccaratnumber_bets", "vipbaccaratnumber_betlogs") },
            { GameId.LuckyNext, new GameTicketCollection("luckynext_bets", "luckynext_betlogs") },
            { GameId.SpeedLuckyNext, new GameTicketCollection("speedluckynext_bets", "speedluckynext_betlogs") },
            { GameId.VIPLuckyNext, new GameTicketCollection("vipluckynext_bets", "vipluckynext_betlogs") },
            { GameId.SeDieNumber, new GameTicketCollection("sedienumber_bets", "sedienumber_betlogs") },
            { GameId.SpeedSeDieNumber, new GameTicketCollection("speedsedienumber_bets", "speedsedienumber_betlogs") },
            { GameId.VIPSeDieNumber, new GameTicketCollection("vipsedienumber_bets", "vipsedienumber_betlogs") },
            { GameId.TripleLuck, new GameTicketCollection("tripleluck_bets", "tripleluck_betlogs") },
            { GameId.SpeedTripleLuck, new GameTicketCollection("speedtripleluck_bets", "speedtripleluck_betlogs") },
            { GameId.VIPTripleLuck, new GameTicketCollection("viptripleluck_bets", "viptripleluck_betlogs") },
            { GameId.SicBoNumber, new GameTicketCollection("sicbonumber_bets", "sicbonumber_betlogs") },
            { GameId.SpeedSicBoNumber, new GameTicketCollection("speedsicbonumber_bets", "speedsicbonumber_betlogs") },
            { GameId.VIPSicBoNumber, new GameTicketCollection("vipsicbonumber_bets", "vipsicbonumber_betlogs") },

            //SK
            { GameId.MegaKeno, new GameTicketCollection( "megakeno_bets", "megakeno_betlogs") },
            { GameId.MegaKeno2, new GameTicketCollection( "megakeno2_bets", "megakeno2_betlogs") },
            { GameId.KenoMini, new GameTicketCollection( "kenomini_bets", "kenomini_betlogs") },
            { GameId.KenoMini2, new GameTicketCollection( "kenomini2_bets", "kenomini2_betlogs") },
            { GameId.KenoEast, new GameTicketCollection( "kenoeast_bets", "kenoeast_betlogs") },
            { GameId.KenoWest, new GameTicketCollection( "kenowest_bets", "kenowest_betlogs") },
            { GameId.KenoSouth, new GameTicketCollection( "kenosouth_bets", "kenosouth_betlogs") },
            { GameId.KenoNorth, new GameTicketCollection( "kenonorth_bets", "kenonorth_betlogs") },
            { GameId.KenoSoccer, new GameTicketCollection( "kenosoccer_bets", "kenosoccer_betlogs") },
            { GameId.KenoSoccer2, new GameTicketCollection( "kenosoccer2_bets", "kenosoccer2_betlogs") },
            { GameId.KenoPower, new GameTicketCollection( "kenopower_bets", "kenopower_betlogs") },
            { GameId.KenoPower2, new GameTicketCollection( "kenopower2_bets", "kenopower2_betlogs") },
           };

        public static GameTicketCollection GetCollections(int gameTypeId, GameType type = GameType.AutoRNG, string displayName = "", bool isCheckBetLog = false)
        {
            if (GameType.Crash == type)
            {
                var betCollection = NamingConverter.BetCollection(displayName);
                var betLogCollection = NamingConverter.BetLogCollection(displayName);
                var primaryCollection = isCheckBetLog ? betCollection : string.Empty;
                var secondaryCollection = isCheckBetLog ? betLogCollection : betCollection;

                return new GameTicketCollection(primaryCollection, secondaryCollection);
            }

            var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

            return gameCollections.TryGetValue(gameId, out var collection) ? collection : default;
        }
    }
}