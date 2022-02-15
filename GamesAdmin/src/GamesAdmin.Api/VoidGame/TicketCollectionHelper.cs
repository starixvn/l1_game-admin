using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using System;
using System.Collections.Generic;

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
            { GameId.KenoProViet, new GameTicketCollection( "kenoproviet_ones", "kenoproviet_cuocs") },
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
            { GameId.CasinoHoldemPoker, new GameTicketCollection( string.Empty, "casinoholdempoker_cuocs") },
        };

        public static GameTicketCollection GetCollections(byte gameTypeId)
        {
            var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

            return gameCollections.ContainsKey(gameId) ? gameCollections[gameId] : default;
        }
    }
}