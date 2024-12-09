using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.VoidGame.BacBall1;
using GamesAdmin.Api.VoidGame.BacBall2;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using GamesAdmin.Database.Entities.Keno;
using GamesAdmin.Database.Entities.KenoProMax;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.VoidGame
{
    public interface ITicketHandler
    {
        Task<bool> Void(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet);

        Task<bool> Exist(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet);

        Task<GameTicketRecord<HoldemPokerBetEntity>> ExistHoldemPoker(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet);

        Task<BlackjackRecord> ExistBlackjackGameTicket<BlackjackGameBet>(string obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity;

        Task<bool> MoveBlackjackGameRound<BlackjackGameRound>(long roundId)
            where BlackjackGameRound : BlackjackGameRoundEntity;
    }

    public class TicketHandler : ITicketHandler
    {
        private const string ArchivePostfix = "_bk";

        private const string RemovePostfix = "_rm";
        private const string RoundResultPostfix = "_results";

        private readonly ISentryClient sentryClient;

        private readonly IMongoDatabase activeDatabase;

        private readonly IMongoDatabase archiveDatabase;

        private readonly IMongoDatabase removedDatabase;

        private readonly IGeneralGameSettingService generalGameSettingService;

        private readonly IBacBall1VoidGameService bacBall1VoidGameService;

        private readonly IBacBall2VoidGameService bacBall2VoidGameService;

        public TicketHandler(
            IAppSettings appSettings,
            ISentryClient sentryClient,
            IGeneralGameSettingService generalGameSettingService,
            IBacBall1VoidGameService bacBall1VoidGameService,
            IBacBall2VoidGameService bacBall2VoidGameService)
        {
            this.sentryClient = sentryClient;
            this.generalGameSettingService = generalGameSettingService;

            var activeClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            activeDatabase = activeClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName);

            var archiveClient = new MongoClient(appSettings.ArchiveDatabaseSettings.ConnectionString);
            archiveDatabase = archiveClient.GetDatabase(appSettings.ArchiveDatabaseSettings.DatabaseName);

            var removedClient = new MongoClient(appSettings.RemovedDatabaseSettings.ConnectionString);
            removedDatabase = removedClient.GetDatabase(appSettings.RemovedDatabaseSettings.DatabaseName);

            this.bacBall1VoidGameService = bacBall1VoidGameService;
            this.bacBall2VoidGameService = bacBall2VoidGameService;
        }

        public async Task<bool> Void(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            try
            {
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());
                var isCrashGame = await generalGameSettingService.IsCrashGame((int)gameId);
                return isCrashGame
                       ? await Move<CrashGameBetEntity, CrashGameBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, GameType.Crash)
                       : gameId switch
                       {
                           GameId.BigSmall => await Move<BigSmallOnesEntity, BigSmallBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.SeDie => await Move<SeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.FishPrawnCrab => await Move<FishPrawnCrabBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.BigSmallTurbo => await Move<BigSmallTurboOnesEntity, BigSmallTurboBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.OddEven => await Move<OddEvenOnesEntity, OddEvenBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.OddEvenTurbo => await Move<OddEvenTurboOnesEntity, OddEvenTurboBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.DragonTiger => await Move<DragonTigerBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoProMax => await Move<KenoProMaxBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.Baccarat => await Move<BaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MiniSeDie => await Move<MiniSeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoMax => await Move<KenoMaxOnesEntity, KenoMaxBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoMax2 => await Move<KenoMax2OnesEntity, KenoMax2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.Roulette => await Move<RouletteOnesEntity, RouletteBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.VipSeDie => await Move<VipSeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.Blackjack => await MoveBlackjackGame<BlackjackBetEntity>(obCustId, partnerId, siteId, gameRoundId, freeBet),
                           GameId.SpecialSeDie => await Move<SpecialSeDieOnesEntity, SpecialSeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.SicBo => await Move<SicBoOnesEntity, SicBoBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.VipFishPrawnCrab => await Move<VipFishPrawnCrabBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.VipBaccarat => await Move<VipBaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.VipSeDieWar => await Move<VipSeDieWarOnesEntity, VipSeDieWarBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.BitcoinMaster => await Move<BitcoinMasterOnesEntity, BitcoinMasterBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.Lottery => await Move<LotteryOnesEntity, LotteryBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.ThreeCards => await Move<ThreeCardsBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.CasinoHoldemPoker => await MoveCasinoHoldem(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.HiLo => await MoveHiLo(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.TexasHoldemPoker => await MoveTexasHoldem(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.BlockchainBaccarat => await Move<BlockchainBaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MiniSeDie2 => await Move<MiniSeDie2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MiniSeDie3 => await Move<MiniSeDie3BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MiniSeDie4 => await Move<MiniSeDie4BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.UnbalancedBigSmall1 => await Move<UnbalancedBigSmall1BetEntity, UnbalancedBigSmall1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.UnbalancedBigSmall2 => await Move<UnbalancedBigSmall2BetEntity, UnbalancedBigSmall2BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.UnbalancedBigSmall3 => await Move<UnbalancedBigSmall3BetEntity, UnbalancedBigSmall3BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.UnbalancedBigSmall4 => await Move<UnbalancedBigSmall4BetEntity, UnbalancedBigSmall4BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.FanTan => await Move<FanTanBetEntity, FanTanBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.FruitSlot => await Move<FruitSlotBetEntity, FruitSlotBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.WinThreeCards => await Move<WinThreeCardsBetEntity, WinThreeCardsBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.DragonTigerFight => await Move<DragonTigerFightBetEntity, DragonTigerFightBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.Pontoon => await MoveBlackjackGame<PontoonBetEntity>(obCustId, partnerId, siteId, gameRoundId, freeBet),
                           GameId.ColorGame => await Move<ColorGameBetEntity, ColorGameBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.BandarQiuQiu => await Move<BandarQiuQiuBetEntity, BandarQiuQiuBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.ChineseBlackjack => await MoveBlackjackGame<ChineseBlackjackBetEntity>(obCustId, partnerId, siteId, gameRoundId, freeBet),
                           GameId.BacBall1 => await Move<BacBall1BetEntity, BacBall1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.BacBall2 => await Move<BacBall2BetEntity, BacBall2BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.LuckyNext1 => await MoveByFieldName<LuckyNext1BetEntity, LuckyNext1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, "DrawId"),
                           GameId.LuckyNext2 => await MoveByFieldName<LuckyNext2BetEntity, LuckyNext2BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, "DrawId"),
                           GameId.FootballDice => await Move<FootballDiceBetEntity, FootballDiceBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.FootballSlot => await Move<FootballSlotBetEntity, FootballSlotBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.CoinToss => await Move<CoinTossBetEntity, CoinTossBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.SeBall1 => await Move<SeBall1BetEntity, SeBall1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.FootballCoinToss => await Move<FootballCoinTossBetEntity, FootballCoinTossBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.LuckyThree => await MoveByFieldName<LuckyThreeBetEntity, LuckyThreeBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, "DrawId"),
                           GameId.SicBall => await Move<SicBallBetEntity, SicBallBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MegaKeno => await Move<MegaKenoOnesEntity, MegaKenoBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MegaKeno2 => await Move<MegaKeno2OnesEntity, MegaKeno2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoMini => await Move<KenoMiniOnesEntity, KenoMiniBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoMini2 => await Move<KenoMini2OnesEntity, KenoMini2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoEast => await Move<KenoEastOnesEntity, KenoEastBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoWest => await Move<KenoWestOnesEntity, KenoWestBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoSouth => await Move<KenoSouthOnesEntity, KenoSouthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoNorth => await Move<KenoNorthOnesEntity, KenoNorthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoSoccer => await Move<KenoMatch1OneEntity, KenoMatch1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoSoccer2 => await Move<KenoMatch2OneEntity, KenoMatch2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoPower => await Move<KenoPower1OneEntity, KenoPower1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoPower2 => await Move<KenoPower2OneEntity, KenoPower2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           _ => false,
                       };
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<GameTicketRecord<HoldemPokerBetEntity>> ExistHoldemPoker(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            try
            {
                return await Find<HoldemPokerBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }
            return default;
        }

        public async Task<bool> Exist(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            try
            {
                IGameTicket gameTicket;
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());
                bool isCrashGame = await generalGameSettingService.IsCrashGame((int)gameId);

                if (isCrashGame)
                {
                    gameTicket = await Find<CrashGameBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, GameType.Crash);
                }
                else
                {
                    gameTicket = gameId switch
                    {
                        GameId.BigSmall => await Find<BigSmallOnesEntity, BigSmallBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.SeDie => await Find<SeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.FishPrawnCrab => await Find<FishPrawnCrabBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BigSmallTurbo => await Find<BigSmallTurboOnesEntity, BigSmallTurboBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.OddEven => await Find<OddEvenOnesEntity, OddEvenBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.OddEvenTurbo => await Find<OddEvenTurboOnesEntity, OddEvenTurboBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.DragonTiger => await Find<DragonTigerBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoProMax => await Find<KenoProMaxBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.Baccarat => await Find<BaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MiniSeDie => await Find<MiniSeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoMax => await Find<KenoMaxOnesEntity, KenoMaxBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoMax2 => await Find<KenoMax2OnesEntity, KenoMax2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.Roulette => await Find<RouletteOnesEntity, RouletteBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.VipSeDie => await Find<VipSeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.SpecialSeDie => await Find<SpecialSeDieOnesEntity, SpecialSeDieBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.SicBo => await Find<SicBoOnesEntity, SicBoBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.VipFishPrawnCrab => await Find<VipFishPrawnCrabBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.VipBaccarat => await Find<VipBaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.VipSeDieWar => await Find<VipSeDieWarOnesEntity, VipSeDieWarBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BitcoinMaster => await Find<BitcoinMasterOnesEntity, BitcoinMasterBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.Lottery => await Find<LotteryOnesEntity, LotteryBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.ThreeCards => await Find<ThreeCardsBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.HiLo => await Find<HiLoBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BlockchainBaccarat => await Find<BlockchainBaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MiniSeDie2 => await Find<MiniSeDie2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MiniSeDie3 => await Find<MiniSeDie3BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MiniSeDie4 => await Find<MiniSeDie4BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.UnbalancedBigSmall1 => await Find<UnbalancedBigSmall1BetEntity, UnbalancedBigSmall1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.UnbalancedBigSmall2 => await Find<UnbalancedBigSmall2BetEntity, UnbalancedBigSmall2BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.UnbalancedBigSmall3 => await Find<UnbalancedBigSmall3BetEntity, UnbalancedBigSmall3BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.UnbalancedBigSmall4 => await Find<UnbalancedBigSmall4BetEntity, UnbalancedBigSmall4BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.FruitSlot => await Find<FruitSlotBetEntity, FruitSlotBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.FanTan => await Find<FanTanBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.WinThreeCards => await Find<WinThreeCardsBetEntity, WinThreeCardsBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.DragonTigerFight => await Find<DragonTigerFightBetEntity, DragonTigerFightBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.ColorGame => await Find<ColorGameBetEntity, ColorGameBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BandarQiuQiu => await Find<BandarQiuQiuBetEntity, BandarQiuQiuBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BacBall1 => await Find<BacBall1BetEntity, BacBall1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BacBall2 => await Find<BacBall2BetEntity, BacBall2BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.LuckyNext1 => await FindByFieldName<LuckyNext1BetEntity, LuckyNext1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, "DrawId"),
                        GameId.LuckyNext2 => await FindByFieldName<LuckyNext2BetEntity, LuckyNext2BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, "DrawId"),
                        GameId.FootballDice => await Find<FootballDiceBetEntity, FootballDiceBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.FootballSlot => await Find<FootballSlotBetEntity, FootballSlotBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.CoinToss => await Find<CoinTossBetEntity, CoinTossBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.SeBall1 => await Find<SeBall1BetEntity, SeBall1BetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.FootballCoinToss => await Find<FootballCoinTossBetEntity, FootballCoinTossBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.LuckyThree => await FindByFieldName<LuckyThreeBetEntity, LuckyThreeBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, "DrawId"),
                        GameId.SicBall => await Find<SicBallBetEntity, SicBallBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MegaKeno => await Find<MegaKenoOnesEntity, MegaKenoBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MegaKeno2 => await Find<MegaKeno2OnesEntity, MegaKeno2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoMini => await Find<KenoMiniOnesEntity, KenoMiniBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoMini2 => await Find<KenoMini2OnesEntity, KenoMini2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoEast => await Find<KenoEastOnesEntity, KenoEastBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoWest => await Find<KenoWestOnesEntity, KenoWestBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoSouth => await Find<KenoSouthOnesEntity, KenoSouthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoNorth => await Find<KenoNorthOnesEntity, KenoNorthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoSoccer => await Find<KenoMatch1OneEntity, KenoMatch1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoSoccer2 => await Find<KenoMatch2OneEntity, KenoMatch2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoPower => await Find<KenoPower1OneEntity, KenoPower1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoPower2 => await Find<KenoPower2OneEntity, KenoPower2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        _ => default,
                    };
                }

                return gameTicket?.Exist() == true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<BlackjackRecord> ExistBlackjackGameTicket<BlackjackGameBet>(string obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity
        => await FindBlackjackGameBet<BlackjackGameBet>(obCustId, partnerId, siteId, gameRoundId, freeBet);

        private async Task<bool> Move<One, Bet>(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet, GameType gameType = GameType.AutoRNG)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            try
            {
                var displayName = await generalGameSettingService.GetDisplayName(gameTypeId);
                var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId, gameType, displayName, true);
                var gameTickets = await Find<One, Bet>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, gameType);

                if (gameTickets.OneRecord != null)
                {
                    var oneCollection = removedDatabase.GetCollection<One>($"{gameCollection.One}{RemovePostfix}");
                    await oneCollection.InsertOneAsync(gameTickets.OneRecord);
                }

                if (gameTickets?.BetRecords?.Any() == true)
                {
                    var betCollection = removedDatabase.GetCollection<Bet>($"{gameCollection.Bet}{RemovePostfix}");
                    await betCollection.InsertManyAsync(gameTickets.BetRecords);
                }

                await Remove<One, Bet>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, gameTickets.IsFromArchive, gameCollection);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        private async Task<bool> Move<Bet>(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
            where Bet : BaseBetEntity
        {
            try
            {
                var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId);
                var gameTicket = await Find<Bet>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);

                if (gameTicket.BetRecords.Count > 0)
                {
                    var betCollection = removedDatabase.GetCollection<Bet>($"{gameCollection.Bet}{RemovePostfix}");
                    await betCollection.InsertManyAsync(gameTicket.BetRecords);
                }

                await Remove<Bet>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, gameTicket.IsFromArchive, gameCollection);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        private async Task<bool> Remove<One, Bet>(
            string obCustId,
            string partnerId,
            string siteId,
            long gameRoundId,
            int gameTypeId,
            bool freeBet,
            bool isFromArchive,
            GameTicketCollection gameCollection)
                where One : BaseBetEntity
                where Bet : BaseBetEntity
        {
            var database = isFromArchive ? archiveDatabase : activeDatabase;
            var postfix = isFromArchive ? ArchivePostfix : string.Empty;

            if (!string.IsNullOrWhiteSpace(gameCollection.One))
            {
                var oneCollection = database.GetCollection<One>($"{gameCollection.One}{postfix}");
                await RemoveTicket(oneCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, gameTypeId);
            }

            if (!string.IsNullOrWhiteSpace(gameCollection.Bet))
            {
                var betCollection = database.GetCollection<Bet>($"{gameCollection.Bet}{postfix}");
                await RemoveTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, gameTypeId);
            }

            return false;
        }

        private async Task<bool> Remove<Bet>(
           string obCustId,
           string partnerId,
           string siteId,
           long gameRoundId,
           int gameTypeId,
           bool freeBet,
           bool isFromArchive,
           GameTicketCollection gameCollection)
               where Bet : BaseBetEntity
        {
            var database = isFromArchive ? archiveDatabase : activeDatabase;
            var postfix = isFromArchive ? ArchivePostfix : string.Empty;

            if (!string.IsNullOrWhiteSpace(gameCollection.Bet))
            {
                var betCollection = database.GetCollection<Bet>($"{gameCollection.Bet}{postfix}");
                await RemoveTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, gameTypeId);
            }

            return false;
        }

        private async Task<GameTicketRecord<One, Bet>> Find<One, Bet>(
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                int gameTypeId,
                bool freeBet,
                GameType gameType = GameType.AutoRNG)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var displayName = await this.generalGameSettingService.GetDisplayName(gameTypeId);
            var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId, gameType, displayName, true);

            var gameTickets = await FindInActiveOrArchive<One, Bet>(gameCollection, obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<One, Bet>(gameCollection, obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, true);
            }

            return gameTickets;
        }

        private async Task<GameTicketRecord<Bet>> Find<Bet>(
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                int gameTypeId,
                bool freeBet,
                GameType gameType = GameType.AutoRNG)
            where Bet : BaseBetEntity
        {
            var displayName = await this.generalGameSettingService.GetDisplayName(gameTypeId);
            var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId, gameType, displayName);

            var gameTickets = await FindInActiveOrArchive<Bet>(gameCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<Bet>(gameCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, true);
            }

            return gameTickets;
        }

        private async Task<GameTicketRecord<One, Bet>> FindInActiveOrArchive<One, Bet>(
                GameTicketCollection collections,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                int gameTypeId,
                bool freeBet,
                bool isArchive = false)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;
            var isCrashGame = await generalGameSettingService.IsCrashGame(gameTypeId);
            var oneCollection = database.GetCollection<One>($"{collections.One}{postfix}");
            var oneRecord = (await FindTicket(oneCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, gameTypeId)).FirstOrDefault();

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var betRecords = await FindTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, gameTypeId);
            var hasTicket = isCrashGame ? oneRecord == null : oneRecord == null && betRecords == null;
            return hasTicket ? default : new GameTicketRecord<One, Bet>(oneRecord, betRecords, isArchive);
        }

        private async Task<GameTicketRecord<Bet>> FindInActiveOrArchive<Bet>(
                GameTicketCollection collections,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                bool isArchive = false)
            where Bet : BaseBetEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;
            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var bets = await FindTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);

            return new GameTicketRecord<Bet>(bets, isArchive);
        }

        private async Task<IList<T>> FindTicket<T>(
                IMongoCollection<T> collection,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                int gameTypeId = 0)
            where T : BaseBetEntity
        {
            List<T> record;
            if (gameTypeId == (int)GameId.BacBall1)
            {
                record = (await bacBall1VoidGameService.FindTicket<T>(collection, obCustId, partnerId, siteId, gameRoundId, freeBet)).ToList();
            }
            else if (gameTypeId == (int)GameId.BacBall2)
            {
                record = (await bacBall2VoidGameService.FindTicket<T>(collection, obCustId, partnerId, siteId, gameRoundId, freeBet)).ToList();
            }
            else
            {
                record = (await collection
                .FindAsync(record => record.PlayerId == obCustId
                                    && record.OperatorId == partnerId
                                    && record.SiteId == siteId
                                    && (record.Round == gameRoundId)
                                    && (!freeBet || record.FreeBet == freeBet)))
                .ToList();
            }

            return record;
        }

        private async Task<bool> RemoveTicket<T>(
                IMongoCollection<T> collection,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                int gameTypeId)
            where T : BaseBetEntity
        {
            DeleteResult result;
            if (gameTypeId == (int)GameId.BacBall1)
            {
                result = await bacBall1VoidGameService.RemoveTicket<T>(collection, obCustId, partnerId, siteId, gameRoundId, freeBet);
            }
            else if (gameTypeId == (int)GameId.BacBall2)
            {
                result = await bacBall2VoidGameService.RemoveTicket<T>(collection, obCustId, partnerId, siteId, gameRoundId, freeBet);
            }
            else
            {
                result = (await collection
                .DeleteManyAsync(record => record.PlayerId == obCustId
                                    && record.SiteId == siteId
                                    && record.OperatorId == partnerId
                                    && (record.Round == gameRoundId)
                                    && (!freeBet || record.FreeBet == freeBet)));
            }

            return result.DeletedCount > 0;
        }

        private async Task<bool> MoveHiLo(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            await Move<HiLoBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);
            return await MoveRound<HiLoRoundEntity>(gameRoundId);
        }

        private async Task<bool> MoveCasinoHoldem(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            await Move<CasinoHoldemPokerBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);
            return await MoveRound<CasinoHoldemPokerRoundEntity>(gameRoundId);
        }

        private async Task<bool> MoveTexasHoldem(string obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            await Move<TexasHoldemPokerBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);
            return await MoveRound<TexasHoldemPokerRoundEntity>(gameRoundId);
        }

        private async Task<bool> MoveRound<RoundEntity>(long gameRoundId)
          where RoundEntity : BaseRoundEntity
        {
            try
            {
                var collectionName = Document.GetCollectionName(typeof(RoundEntity));
                var collection = activeDatabase.GetCollection<RoundEntity>(collectionName);

                var round = await collection.Find(x => x.Number == gameRoundId).FirstOrDefaultAsync();

                if (round != null)
                {
                    await collection.DeleteOneAsync(x => x.Number == gameRoundId);
                }
                else
                {
                    var archiveCollection = archiveDatabase.GetCollection<RoundEntity>($"{collectionName}{ArchivePostfix}");
                    round = await archiveCollection.Find(x => x.Number == gameRoundId).FirstOrDefaultAsync();
                    await archiveCollection.DeleteOneAsync(x => x.Number == gameRoundId);
                }

                var removeCollection = removedDatabase.GetCollection<RoundEntity>($"{collectionName}{RemovePostfix}");
                await removeCollection.InsertOneAsync(round);

                await MoveResult(gameRoundId, collectionName.Split("_").FirstOrDefault());

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        private async Task MoveResult(long gameRoundId, string gameName)
        {
            var resultCollectionName = $"{gameName}{RoundResultPostfix}";
            var resultCollection = activeDatabase.GetCollection<dynamic>(resultCollectionName);
            var result = await resultCollection.Find(new BsonDocument("roundId", gameRoundId)).FirstOrDefaultAsync();

            if (result != null)
            {
                await resultCollection.DeleteOneAsync(new BsonDocument("roundId", gameRoundId));
                await removedDatabase
                    .GetCollection<dynamic>($"{resultCollectionName}{RemovePostfix}")
                    .InsertOneAsync(result);
            }
        }

        private async Task<BlackjackRecord> FindBlackjackGameBet<BlackjackGameBet>(string obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity
        {
            var isArchive = false;
            var bet = new BlackjackGameBetEntity();
            var allTicket = new List<long>();
            try
            {
                var collectionName = Document.GetCollectionName(typeof(BlackjackGameBet));
                var collection = activeDatabase.GetCollection<BlackjackGameBet>(collectionName);

                bet = await FindBlackjackTicket(collection, obCustId, partnerId, siteId, gameRoundId, freeBet);

                if (bet == null)
                {
                    isArchive = true;
                    var archiveCollection = archiveDatabase.GetCollection<BlackjackGameBet>($"{collectionName}{ArchivePostfix}");
                    bet = await FindBlackjackTicket(archiveCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);
                }

                if (bet != null)
                {
                    allTicket = await FindAllTicketBlackjack<BlackjackGameBet>(bet?.Round, isArchive);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return new BlackjackRecord(bet, isArchive, allTicket.ToList());
        }

        private async Task<bool> MoveBlackjackGame<BlackjackGameBet>(string obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity
        {
            try
            {
                var ticket = await FindBlackjackGameBet<BlackjackGameBet>(obCustId, partnerId, siteId, gameRoundId, freeBet);
                var collectionName = Document.GetCollectionName(typeof(BlackjackGameBet));
                if (ticket.BetRecord != null)
                {
                    var betCollection = removedDatabase.GetCollection<BlackjackGameBetEntity>($"{collectionName}{RemovePostfix}");
                    await betCollection.InsertOneAsync(ticket.BetRecord);
                    var database = ticket.IsFromArchive ? archiveDatabase : activeDatabase;
                    var postfix = ticket.IsFromArchive ? ArchivePostfix : string.Empty;
                    var collection = database.GetCollection<BlackjackGameBetEntity>($"{collectionName}{postfix}");
                    var result = await collection
                    .DeleteOneAsync(record => record.PlayerId == obCustId
                                        && record.SiteId == siteId
                                        && record.OperatorId == partnerId
                                        && (record.HandId == gameRoundId)
                                        && (!freeBet || record.FreeBet == freeBet));
                    return result.DeletedCount > 0;
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> MoveBlackjackGameRound<BlackjackGameRound>(long roundId)
            where BlackjackGameRound : BlackjackGameRoundEntity
        => await MoveRound<BlackjackGameRound>(roundId);

        public async Task<List<long>> FindAllTicketBlackjack<BlackjackGameBet>(long? roundId, bool isArchive)
            where BlackjackGameBet : BlackjackGameBetEntity
        {
            try
            {
                var database = isArchive ? archiveDatabase : activeDatabase;
                var postfix = isArchive ? ArchivePostfix : string.Empty;
                var betCollectionName = Document.GetCollectionName(typeof(BlackjackGameBet));

                var betCollection = database.GetCollection<BlackjackGameBet>($"{betCollectionName}{postfix}");
                var bets = (await betCollection.FindAsync(record => record.Round == roundId)).ToList();
                if (bets.Count > 0)
                {
                    return bets.Select(bet => { return bet.HandId; }).ToList();
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return new List<long>();
        }

        private static async Task<BlackjackGameBet> FindBlackjackTicket<BlackjackGameBet>(
              IMongoCollection<BlackjackGameBet> collection,
              string obCustId,
              string partnerId,
              string siteId,
              long gameRoundId,
              bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity
        {
            var bet = (await collection
                .FindAsync(record => record.PlayerId == obCustId
                                    && record.OperatorId == partnerId
                                    && record.SiteId == siteId
                                    && record.HandId == gameRoundId
                                    && (!freeBet || record.FreeBet == freeBet)))
                .FirstOrDefault();

            return bet;
        }

        private async Task<IList<T>> FindTicketByFieldName<T>(
                IMongoCollection<T> collection,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                string findFieldName)
            where T : BaseBetEntity
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq("PlayerId", obCustId)
                & builder.Eq("OperatorId", partnerId)
                & builder.Eq("SiteId", siteId)
                & builder.Eq(findFieldName, gameRoundId);
            if (freeBet)
            {
                filter &= builder.Eq("FreeBet", freeBet);
            }
            return (await collection.FindAsync(filter)).ToList();
        }

        private async Task<GameTicketRecord<One, Bet>> FindInActiveOrArchiveByFieldName<One, Bet>(
                GameTicketCollection collections,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                string findFieldName,
                bool isArchive = false)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;
            var oneCollection = database.GetCollection<One>($"{collections.One}{postfix}");
            var oneRecord = (await FindTicketByFieldName(oneCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, findFieldName)).FirstOrDefault();

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var betRecords = await FindTicketByFieldName(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, findFieldName);
            return new GameTicketRecord<One, Bet>(oneRecord, betRecords, isArchive);
        }

        private async Task<GameTicketRecord<One, Bet>> FindByFieldName<One, Bet>(
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                int gameTypeId,
                bool freeBet,
                string findFieldName,
                GameType gameType = GameType.AutoRNG)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var displayName = await this.generalGameSettingService.GetDisplayName(gameTypeId);
            var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId, gameType, displayName, true);

            var gameTickets = await FindInActiveOrArchiveByFieldName<One, Bet>(gameCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, findFieldName);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchiveByFieldName<One, Bet>(gameCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, findFieldName, true);
            }

            return gameTickets;
        }

        private async Task<bool> RemoveTicketByFieldName<T>(
                IMongoCollection<T> collection,
                string obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                string findFieldName)
            where T : BaseBetEntity
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq("PlayerId", obCustId)
                & builder.Eq("OperatorId", partnerId)
                & builder.Eq("SiteId", siteId)
                & builder.Eq(findFieldName, gameRoundId);
            if (freeBet)
            {
                filter &= builder.Eq("FreeBet", freeBet);
            }
            var result = (await collection
                .DeleteManyAsync(filter));

            return result.DeletedCount > 0;
        }

        private async Task<bool> RemoveByFieldName<One, Bet>(
            string obCustId,
            string partnerId,
            string siteId,
            long gameRoundId,
            bool freeBet,
            bool isFromArchive,
            GameTicketCollection gameCollection,
            string findFieldName)
                where One : BaseBetEntity
                where Bet : BaseBetEntity
        {
            var database = isFromArchive ? archiveDatabase : activeDatabase;
            var postfix = isFromArchive ? ArchivePostfix : string.Empty;

            if (!string.IsNullOrWhiteSpace(gameCollection.One))
            {
                var oneCollection = database.GetCollection<One>($"{gameCollection.One}{postfix}");
                await RemoveTicketByFieldName(oneCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, findFieldName);
            }

            if (!string.IsNullOrWhiteSpace(gameCollection.Bet))
            {
                var betCollection = database.GetCollection<Bet>($"{gameCollection.Bet}{postfix}");
                await RemoveTicketByFieldName(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet, findFieldName);
            }

            return false;
        }

        private async Task<bool> MoveByFieldName<One, Bet>(
            string obCustId,
            string partnerId,
            string siteId,
            long gameRoundId,
            int gameTypeId,
            bool freeBet,
            string findFieldName,
            GameType gameType = GameType.AutoRNG)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            try
            {
                var displayName = await generalGameSettingService.GetDisplayName(gameTypeId);
                var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId, gameType, displayName, true);
                var gameTickets = await FindByFieldName<One, Bet>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet, findFieldName, gameType);

                if (gameTickets.OneRecord != null)
                {
                    var oneCollection = removedDatabase.GetCollection<One>($"{gameCollection.One}{RemovePostfix}");
                    await oneCollection.InsertOneAsync(gameTickets.OneRecord);
                }

                if (gameTickets?.BetRecords?.Any() == true)
                {
                    var betCollection = removedDatabase.GetCollection<Bet>($"{gameCollection.Bet}{RemovePostfix}");
                    await betCollection.InsertManyAsync(gameTickets.BetRecords);
                }

                await RemoveByFieldName<One, Bet>(obCustId, partnerId, siteId, gameRoundId, freeBet, gameTickets.IsFromArchive, gameCollection, findFieldName);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }
    }
}