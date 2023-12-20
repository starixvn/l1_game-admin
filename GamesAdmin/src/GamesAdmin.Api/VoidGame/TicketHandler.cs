using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Baccarat;
using GamesAdmin.Database.Entities.Keno;
using GamesAdmin.Database.Entities.KenoProMax;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.VoidGame
{
    public interface ITicketHandler
    {
        Task<bool> Void(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet);

        Task<bool> Exist(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet);

        Task<GameTicketRecord<HoldemPokerBetEntity>> ExistHoldemPoker(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet);

        Task<BlackjackRecord> ExistBlackjackGameTicket<BlackjackGameBet>(int obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity;

        Task<bool> MoveBlackjackGameRound<BlackjackGameRound>(long roundId)
            where BlackjackGameRound : BlackjackGameRoundEntity;
    }

    public class TicketHandler : ITicketHandler
    {
        private const string ArchivePostfix = "_bk";

        private const string RemovePostfix = "_rm";

        private readonly ISentryClient sentryClient;

        private readonly IMongoDatabase activeDatabase;

        private readonly IMongoDatabase archiveDatabase;

        private readonly IMongoDatabase removedDatabase;

        private readonly IGeneralGameSettingService generalGameSettingService;

        public TicketHandler(IAppSettings appSettings, ISentryClient sentryClient, IGeneralGameSettingService generalGameSettingService)
        {
            this.sentryClient = sentryClient;
            this.generalGameSettingService = generalGameSettingService;

            var activeClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            activeDatabase = activeClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName);

            var archiveClient = new MongoClient(appSettings.ArchiveDatabaseSettings.ConnectionString);
            archiveDatabase = archiveClient.GetDatabase(appSettings.ArchiveDatabaseSettings.DatabaseName);

            var removedClient = new MongoClient(appSettings.RemovedDatabaseSettings.ConnectionString);
            removedDatabase = removedClient.GetDatabase(appSettings.RemovedDatabaseSettings.DatabaseName);
        }

        public async Task<bool> Void(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
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
                           GameId.KenoMini => await Move<KenoMiniOnesEntity, KenoMiniBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoMini2 => await Move<KenoMini2OnesEntity, KenoMini2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoEast => await Move<KenoEastOnesEntity, KenoEastBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoWest => await Move<KenoWestOnesEntity, KenoWestBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoSouth => await Move<KenoSouthOnesEntity, KenoSouthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoNorth => await Move<KenoNorthOnesEntity, KenoNorthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
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
                           GameId.KenoMatch1 => await Move<KenoMatch1OneEntity, KenoMatch1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoMatch2 => await Move<KenoMatch2OneEntity, KenoMatch2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoPower1 => await Move<KenoPower1OneEntity, KenoPower1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.KenoPower2 => await Move<KenoPower2OneEntity, KenoPower2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.CasinoHoldemPoker => await MoveCasinoHoldem(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.HiLo => await MoveHiLo(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.TexasHoldemPoker => await MoveTexasHoldem(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.BlockchainBaccarat => await Move<BlockchainBaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MegaKeno => await Move<MegaKenoOnesEntity, MegaKenoBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                           GameId.MegaKeno2 => await Move<MegaKeno2OnesEntity, MegaKeno2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
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
                           _ => false,
                       };
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<GameTicketRecord<HoldemPokerBetEntity>> ExistHoldemPoker(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
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

        public async Task<bool> Exist(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            try
            {
                IGameTicket gameTicket;
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());
                var isCrashGame = await generalGameSettingService.IsCrashGame((int)gameId);

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
                        GameId.KenoMini => await Find<KenoMiniOnesEntity, KenoMiniBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoMini2 => await Find<KenoMini2OnesEntity, KenoMini2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoEast => await Find<KenoEastOnesEntity, KenoEastBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoWest => await Find<KenoWestOnesEntity, KenoWestBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoSouth => await Find<KenoSouthOnesEntity, KenoSouthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoNorth => await Find<KenoNorthOnesEntity, KenoNorthBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
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
                        GameId.KenoMatch1 => await Find<KenoMatch1OneEntity, KenoMatch1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoMatch2 => await Find<KenoMatch2OneEntity, KenoMatch2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoPower1 => await Find<KenoPower1OneEntity, KenoPower1BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.KenoPower2 => await Find<KenoPower2OneEntity, KenoPower2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.HiLo => await Find<HiLoBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.BlockchainBaccarat => await Find<BlockchainBaccaratBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MegaKeno => await Find<MegaKenoOnesEntity, MegaKenoBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
                        GameId.MegaKeno2 => await Find<MegaKeno2OnesEntity, MegaKeno2BetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet),
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

        public async Task<BlackjackRecord> ExistBlackjackGameTicket<BlackjackGameBet>(int obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
            where BlackjackGameBet : BlackjackGameBetEntity
        => await FindBlackjackGameBet<BlackjackGameBet>(obCustId, partnerId, siteId, gameRoundId, freeBet);

        private async Task<bool> Move<One, Bet>(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet, GameType gameType = GameType.AutoRNG)
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

        private async Task<bool> Move<Bet>(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
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
            int obCustId,
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
                await RemoveTicket(oneCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);
            }

            if (!string.IsNullOrWhiteSpace(gameCollection.Bet))
            {
                var betCollection = database.GetCollection<Bet>($"{gameCollection.Bet}{postfix}");
                await RemoveTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);
            }

            return false;
        }

        private async Task<bool> Remove<Bet>(
           int obCustId,
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
                await RemoveTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);
            }

            return false;
        }

        private async Task<GameTicketRecord<One, Bet>> Find<One, Bet>(
                int obCustId,
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
                int obCustId,
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
                int obCustId,
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
            var oneRecord = (await FindTicket(oneCollection, obCustId, partnerId, siteId, gameRoundId, freeBet)).FirstOrDefault();

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var betRecords = await FindTicket(betCollection, obCustId, partnerId, siteId, gameRoundId, freeBet);
            var hasTicket = isCrashGame ? oneRecord == null : oneRecord == null && betRecords == null;
            return hasTicket ? default : new GameTicketRecord<One, Bet>(oneRecord, betRecords, isArchive);
        }

        private async Task<GameTicketRecord<Bet>> FindInActiveOrArchive<Bet>(
                GameTicketCollection collections,
                int obCustId,
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

        private static async Task<IList<T>> FindTicket<T>(
                IMongoCollection<T> collection,
                int obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity
        {
            var record = (await collection
                .FindAsync(record => record.PlayerId == obCustId
                                    && record.OperatorId == partnerId
                                    && record.SiteId == siteId
                                    && (record.Round == gameRoundId)
                                    && (!freeBet || record.FreeBet == freeBet)))
                .ToList();

            return record;
        }

        private static async Task<bool> RemoveTicket<T>(
                IMongoCollection<T> collection,
                int obCustId,
                string partnerId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity
        {
            var result = (await collection
                .DeleteManyAsync(record => record.PlayerId == obCustId
                                    && record.SiteId == siteId
                                    && record.OperatorId == partnerId
                                    && (record.Round == gameRoundId)
                                    && (!freeBet || record.FreeBet == freeBet)));

            return result.DeletedCount > 0;
        }

        private async Task<bool> MoveHiLo(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            await Move<HiLoBetLogEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);
            return await MoveRound<HiLoRoundEntity>(gameRoundId);
        }

        private async Task<bool> MoveCasinoHoldem(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
        {
            await Move<CasinoHoldemPokerBetEntity>(obCustId, partnerId, siteId, gameRoundId, gameTypeId, freeBet);
            return await MoveRound<CasinoHoldemPokerRoundEntity>(gameRoundId);
        }

        private async Task<bool> MoveTexasHoldem(int obCustId, string partnerId, string siteId, long gameRoundId, int gameTypeId, bool freeBet)
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

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        private async Task<BlackjackRecord> FindBlackjackGameBet<BlackjackGameBet>(int obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
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

        private async Task<bool> MoveBlackjackGame<BlackjackGameBet>(int obCustId, string partnerId, string siteId, long gameRoundId, bool freeBet)
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
              int obCustId,
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
    }
}