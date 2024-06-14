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
            { GameId.KenoMini, new GameTicketCollection( "kenomini_ones", "kenomini_cuocs") },
            { GameId.KenoMini2, new GameTicketCollection( "kenomini2_ones", "kenomini2_cuocs") },
            { GameId.KenoEast, new GameTicketCollection( "kenoeast_ones", "kenoeast_cuocs") },
            { GameId.KenoWest, new GameTicketCollection( "kenowest_ones", "kenowest_cuocs") },
            { GameId.KenoSouth, new GameTicketCollection( "kenosouth_ones", "kenosouth_cuocs") },
            { GameId.KenoNorth, new GameTicketCollection( "kenonorth_ones", "kenonorth_cuocs") },
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
            { GameId.KenoMatch1, new GameTicketCollection( "kenomatch1_ones", "kenomatch1_cuocs") },
            { GameId.KenoMatch2, new GameTicketCollection( "kenomatch2_ones", "kenomatch2_cuocs") },
            { GameId.KenoPower1, new GameTicketCollection( "kenopower1_ones", "kenopower1_cuocs") },
            { GameId.KenoPower2, new GameTicketCollection( "kenopower2_ones", "kenopower2_cuocs") },
            { GameId.BitcoinMaster, new GameTicketCollection( "bitcoinmaster_bets", "bitcoinmaster_betlogs") },
            { GameId.CasinoHoldemPoker, new GameTicketCollection( "casinoholdempoker_phiens", "casinoholdempoker_cuocs") },
            { GameId.HiLo, new GameTicketCollection( string.Empty, "hilo_betlogs") },
            { GameId.TexasHoldemPoker, new GameTicketCollection( "texasholdempoker_phiens", "texasholdempoker_cuocs") },
            { GameId.BlockchainBaccarat, new GameTicketCollection( string.Empty, "blockchainbaccarat_bets") },
            { GameId.MegaKeno, new GameTicketCollection( "megakeno_ones", "megakeno_bets") },
            { GameId.MegaKeno2, new GameTicketCollection( "megakeno2_ones", "megakeno2_bets") },
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
            { GameId.BacBall1, new GameTicketCollection("bacball1_bets", "bacball1_betlogs") },
            { GameId.BacBall2, new GameTicketCollection("bacball2_bets", "bacball2_betlogs") },
            { GameId.LuckyNext1, new GameTicketCollection("luckynext1_bets", "luckynext1_betlogs") },
            { GameId.LuckyNext2, new GameTicketCollection("luckynext2_bets", "luckynext2_betlogs") },
            { GameId.FootballDice, new GameTicketCollection("footballdice_bets", "footballdice_betlogs") },
            { GameId.FootballSlot, new GameTicketCollection("footballslot_bets", "footballslot_betlogs") },
            { GameId.CoinToss, new GameTicketCollection("cointoss_bets", "cointoss_betlogs") },
            { GameId.SeBall1, new GameTicketCollection("seball1_bets", "seball1_betlogs") },
            { GameId.FootballCoinToss, new GameTicketCollection("footballcointoss_bets", "footballcointoss_betlogs") },
            { GameId.LuckyThree, new GameTicketCollection("luckythree_bets", "luckythree_betlogs") },
            { GameId.Bac81, new GameTicketCollection("bac81_bets", "bac81_betlogs") },
            { GameId.SicBall, new GameTicketCollection("sicball_bets", "sicball_betlogs") },
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