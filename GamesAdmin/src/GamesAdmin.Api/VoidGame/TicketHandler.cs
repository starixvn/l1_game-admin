using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
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
        Task<GameTicketRecord<T1, T2>> Find<T1, T2>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where T1 : BaseBetEntity
            where T2 : BaseBetEntity;

        Task<bool> Move<T1, T2>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where T1 : BaseBetEntity
            where T2 : BaseBetEntity;

        Task<GameRoundTicketRecord<Round, Bet>> FindRoundAndBet<Round, Bet>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
           where Round : BaseRoundEntity
           where Bet : BaseBetEntity;

        Task<bool> Void(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet);
    }

    public class TicketHandler : ITicketHandler
    {
        private const string ArchivePostfix = "_bk";
        private const string RemovePostfix = "_rm";

        private static Dictionary<GameId, GameTicketCollection> gameCollections = new Dictionary<GameId, GameTicketCollection>
        {
            { GameId.BigSmall, new GameTicketCollection(string.Empty, "taixiu_ones", "taixiu_cuocs") },
            { GameId.SeDie, new GameTicketCollection(string.Empty, string.Empty, "xocxoc_cuocs") },
            { GameId.FishPrawnCrab, new GameTicketCollection(string.Empty, string.Empty, "baucua_cuocs") },
            { GameId.BigSmallTurbo, new GameTicketCollection(string.Empty, "txturbo_ones", "txturbo_cuocs") },
            { GameId.OddEven, new GameTicketCollection(string.Empty, "chanle_ones", "chanle_cuocs") },
            { GameId.OddEvenTurbo, new GameTicketCollection(string.Empty, "chanleturbo_ones", "chanleturbo_cuocs") },
            { GameId.DragonTiger, new GameTicketCollection(string.Empty, string.Empty, "minidragontiger_cuocs") },
            { GameId.KenoProMax, new GameTicketCollection(string.Empty, string.Empty, "kenopromax_cuocs") },
            { GameId.MiniSeDie, new GameTicketCollection(string.Empty, string.Empty, "minisedie_cuocs") },
            { GameId.Baccarat, new GameTicketCollection(string.Empty, string.Empty, "baccarat_cuocs") },
            { GameId.KenoProViet, new GameTicketCollection(string.Empty, "kenoproviet_ones", "kenoproviet_cuocs") },
            { GameId.KenoMax, new GameTicketCollection(string.Empty, "kenomax_ones", "kenomax_cuocs") },
            { GameId.KenoMax2, new GameTicketCollection(string.Empty, "kenomax2_ones", "kenomax2_cuocs") },
            { GameId.KenoMini, new GameTicketCollection(string.Empty, "kenomini_ones", "kenomini_cuocs") },
            { GameId.KenoMini2, new GameTicketCollection(string.Empty, "kenomini2_ones", "kenomini2_cuocs") },
            { GameId.KenoEast, new GameTicketCollection(string.Empty, "kenoeast_ones", "kenoeast_cuocs") },
            { GameId.KenoWest, new GameTicketCollection(string.Empty, "kenowest_ones", "kenowest_cuocs") },
            { GameId.KenoSouth, new GameTicketCollection(string.Empty, "kenosouth_ones", "kenosouth_cuocs") },
            { GameId.KenoNorth, new GameTicketCollection(string.Empty, "kenonorth_ones", "kenonorth_cuocs") },
            { GameId.Roulette, new GameTicketCollection(string.Empty, "roulette_ones", "roulette_cuocs") },
            { GameId.VipSeDie, new GameTicketCollection(string.Empty, string.Empty, "vipsedie_cuocs") },
            { GameId.Blackjack, new GameTicketCollection("blackjack_phiens", string.Empty, "blackjack_cuocs") },
            { GameId.SpecialSeDie, new GameTicketCollection(string.Empty, "specialsedie_ones", "specialsedie_cuocs") },
            { GameId.SicBo, new GameTicketCollection(string.Empty, "sicbo_ones", "sicbo_cuocs") },
            { GameId.VipFishPrawnCrab, new GameTicketCollection(string.Empty, string.Empty, "vipfishprawncrab_cuocs") },
            { GameId.VipBaccarat, new GameTicketCollection(string.Empty, string.Empty, "vipbaccarat_cuocs") },
            { GameId.VipSeDieWar, new GameTicketCollection(string.Empty, "vipsediewar_ones", "vipsediewar_cuocs") },
        };

        private readonly ISentryClient sentryClient;
        private readonly IMongoDatabase activeDatabase;
        private readonly IMongoDatabase archiveDatabase;
        private readonly IMongoDatabase removedDatabase;

        public TicketHandler(IAppSettings appSettings, ISentryClient sentryClient)
        {
            this.sentryClient = sentryClient;

            var activeClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            activeDatabase = activeClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName);

            var archiveClient = new MongoClient(appSettings.ArchiveDatabaseSettings.ConnectionString);
            archiveDatabase = archiveClient.GetDatabase(appSettings.ArchiveDatabaseSettings.DatabaseName);

            var removedClient = new MongoClient(appSettings.RemovedDatabaseSettings.ConnectionString);
            removedDatabase = removedClient.GetDatabase(appSettings.RemovedDatabaseSettings.DatabaseName);
        }

        public async Task<bool> Void(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
        {
            try
            {
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

                return gameId switch
                {
                    GameId.BigSmall => await Move<BigSmallOnesEntity, BigSmallBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.SeDie => await Move<SeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.FishPrawnCrab => await Move<FishPrawnCrabBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.BigSmallTurbo => await Move<BigSmallTurboOnesEntity, BigSmallTurboBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.OddEven => await Move<OddEvenOnesEntity, OddEvenBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.OddEvenTurbo => await Move<OddEvenTurboOnesEntity, OddEvenTurboBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.DragonTiger => await Move<DragonTigerBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoProMax => await Move<KenoProMaxBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.Baccarat => await Move<BaccaratBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.MiniSeDie => await Move<MiniSeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoProViet => await Move<KenoProVietOnesEntity, KenoProVietBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMax => await Move<KenoMaxOnesEntity, KenoMaxBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMax2 => await Move<KenoMax2OnesEntity, KenoMax2BetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMini => await Move<KenoMiniOnesEntity, KenoMiniBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMini2 => await Move<KenoMini2OnesEntity, KenoMini2BetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoEast => await Move<KenoEastOnesEntity, KenoEastBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoWest => await Move<KenoWestOnesEntity, KenoWestBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoSouth => await Move<KenoSouthOnesEntity, KenoSouthBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoNorth => await Move<KenoNorthOnesEntity, KenoNorthBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.Roulette => await Move<RouletteOnesEntity, RouletteBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipSeDie => await Move<VipSeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.Blackjack => await MoveRoundAndBet<BlackjackRoundEntity, BlackjackBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.SpecialSeDie => await Move<SpecialSeDieOnesEntity, SpecialSeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.SicBo => await Move<SicBoOnesEntity, SicBoBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipFishPrawnCrab => await Move<VipFishPrawnCrabBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipBaccarat => await Move<VipBaccaratBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipSeDieWar => await Move<VipSeDieWarOnesEntity, VipSeDieWarBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    _ => false,
                };
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> Move<One, Bet>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            try
            {
                var gameCollection = GetCollections(gameTypeId);
                var gameTickets = await Find<One, Bet>(obCustId, siteId, gameRoundId, gameTypeId, freeBet);

                if (gameTickets.OneRecord != null)
                {
                    var oneCollection = removedDatabase.GetCollection<One>($"{gameCollection.One}{RemovePostfix}");
                    await oneCollection.InsertOneAsync(gameTickets.OneRecord);
                }

                if (gameTickets.BetRecords != null)
                {
                    var betCollection = removedDatabase.GetCollection<Bet>($"{gameCollection.Bet}{RemovePostfix}");
                    await betCollection.InsertManyAsync(gameTickets.BetRecords);
                }

                await Remove<One, Bet>(obCustId, siteId, gameRoundId, gameTypeId, freeBet, gameTickets.IsFromArchive, gameCollection);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> Move<Bet>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where Bet : BaseBetEntity
        {
            try
            {
                var gameCollection = GetCollections(gameTypeId);
                var gameTicket = await Find<Bet>(obCustId, siteId, gameRoundId, gameTypeId, freeBet);

                if (gameTicket.BetRecords.Count > 0)
                {
                    var betCollection = removedDatabase.GetCollection<Bet>($"{gameCollection.Bet}{RemovePostfix}");
                    await betCollection.InsertManyAsync(gameTicket.BetRecords);
                }

                await Remove<Bet>(obCustId, siteId, gameRoundId, gameTypeId, freeBet, gameTicket.IsFromArchive, gameCollection);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> MoveRoundAndBet<Round, Bet>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where Round : BaseRoundEntity
            where Bet : BaseBetEntity
        {
            try
            {
                var gameCollection = GetCollections(gameTypeId);
                var gameTickets = await FindRoundAndBet<Round, Bet>(obCustId, siteId, gameRoundId, gameTypeId, freeBet);

                if (gameTickets.RoundRecord != null)
                {
                    var roundCollection = removedDatabase.GetCollection<Round>($"{gameCollection.Round}{RemovePostfix}");
                    await roundCollection.InsertOneAsync(gameTickets.RoundRecord);
                }

                if (gameTickets.BetRecords != null)
                {
                    var betCollection = removedDatabase.GetCollection<Bet>($"{gameCollection.Bet}{RemovePostfix}");
                    await betCollection.InsertManyAsync(gameTickets.BetRecords);
                }

                await Remove<Bet>(obCustId, siteId, gameRoundId, gameTypeId, freeBet, gameTickets.IsFromArchive, gameCollection);
                await RemoveRound<Round>(gameRoundId, gameTickets.IsFromArchive, gameCollection);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> Remove<One, Bet>(
            int obCustId,
            string siteId,
            long gameRoundId,
            byte gameTypeId,
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
                await RemoveTicket(oneCollection, obCustId, siteId, gameRoundId, freeBet);
            }

            if (!string.IsNullOrWhiteSpace(gameCollection.Bet))
            {
                var betCollection = database.GetCollection<Bet>($"{gameCollection.Bet}{postfix}");
                await RemoveTicket(betCollection, obCustId, siteId, gameRoundId, freeBet);
            }

            return false;
        }

        public async Task<bool> Remove<Bet>(
           int obCustId,
           string siteId,
           long gameRoundId,
           byte gameTypeId,
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
                await RemoveTicket(betCollection, obCustId, siteId, gameRoundId, freeBet);
            }

            return false;
        }

        public async Task<bool> RemoveRound<Round>(
           long gameRoundId,
           bool isFromArchive,
           GameTicketCollection gameCollection)
               where Round : BaseRoundEntity
        {
            var database = isFromArchive ? archiveDatabase : activeDatabase;
            var postfix = isFromArchive ? ArchivePostfix : string.Empty;

            if (!string.IsNullOrWhiteSpace(gameCollection.Round))
            {
                var betCollection = database.GetCollection<Round>($"{gameCollection.Round}{postfix}");
                return await RemoveRound(betCollection, gameRoundId);
            }

            return false;
        }

        public async Task<bool> RemoveRoundAndBet<Round, Bet>(
            int obCustId,
            string siteId,
            long gameRoundId,
            byte gameTypeId,
            bool freeBet,
            bool isFromArchive,
            GameTicketCollection gameCollection)
                where Round : BaseRoundEntity
                where Bet : BaseBetEntity
        {
            var database = isFromArchive ? archiveDatabase : activeDatabase;
            var postfix = isFromArchive ? ArchivePostfix : string.Empty;

            if (!string.IsNullOrWhiteSpace(gameCollection.Round))
            {
                var roundCollection = database.GetCollection<Round>($"{gameCollection.Round}{postfix}");
                await RemoveRound(roundCollection, gameRoundId);
            }

            if (!string.IsNullOrWhiteSpace(gameCollection.Bet))
            {
                var betCollection = database.GetCollection<Bet>($"{gameCollection.Bet}{postfix}");
                await RemoveTicket(betCollection, obCustId, siteId, gameRoundId, freeBet);
            }

            return false;
        }

        public async Task<GameTicketRecord<One, Bet>> Find<One, Bet>(
                int obCustId,
                string siteId,
                long gameRoundId,
                byte gameTypeId,
                bool freeBet)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var gameCollection = GetCollections(gameTypeId);

            var gameTickets = await FindInActiveOrArchive<One, Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<One, Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet, true);
            }

            return gameTickets;
        }

        public async Task<GameTicketRecord<Bet>> Find<Bet>(
                int obCustId,
                string siteId,
                long gameRoundId,
                byte gameTypeId,
                bool freeBet)
            where Bet : BaseBetEntity
        {
            var gameCollection = GetCollections(gameTypeId);

            var gameTickets = await FindInActiveOrArchive<Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet, true);
            }

            return gameTickets;
        }

        public async Task<GameRoundTicketRecord<Round, Bet>> FindRoundAndBet<Round, Bet>(
               int obCustId,
               string siteId,
               long gameRoundId,
               byte gameTypeId,
               bool freeBet)
           where Round : BaseRoundEntity
           where Bet : BaseBetEntity
        {
            var gameCollection = GetCollections(gameTypeId);

            var gameTickets = await FindRoundInActiveOrArchive<Round, Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindRoundInActiveOrArchive<Round, Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet, true);
            }

            return gameTickets;
        }

        private async Task<GameTicketRecord<One, Bet>> FindInActiveOrArchive<One, Bet>(
                GameTicketCollection collections,
                int obCustId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                bool isArchive = false)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;

            var oneCollection = database.GetCollection<One>($"{collections.One}{postfix}");
            var oneRecord = await FindTicket(oneCollection, obCustId, siteId, gameRoundId, freeBet);

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var betRecords = await FindTicketList(betCollection, obCustId, siteId, gameRoundId, freeBet);

            return oneRecord == null && betRecords == null ? default : new GameTicketRecord<One, Bet>(oneRecord, betRecords, isArchive);
        }

        private async Task<GameTicketRecord<Bet>> FindInActiveOrArchive<Bet>(
                GameTicketCollection collections,
                int obCustId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                bool isArchive = false)
            where Bet : BaseBetEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var bets = await FindTicketList(betCollection, obCustId, siteId, gameRoundId, freeBet);

            return new GameTicketRecord<Bet>(bets, isArchive);
        }

        private async Task<GameRoundTicketRecord<Round, Bet>> FindRoundInActiveOrArchive<Round, Bet>(
                GameTicketCollection collections,
                int obCustId,
                string siteId,
                long gameRoundId,
                bool freeBet,
                bool isArchive = false)
            where Round : BaseRoundEntity
            where Bet : BaseBetEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;

            var roundCollection = database.GetCollection<Round>($"{collections.Round}{postfix}");
            var round = await FindRound(roundCollection, gameRoundId);

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var bets = await FindTicketList(betCollection, obCustId, siteId, gameRoundId, freeBet);

            return new GameRoundTicketRecord<Round, Bet>(round, bets, isArchive);
        }

        private async Task<T> FindRound<T>(IMongoCollection<T> collection, long gameRoundId)
            where T : BaseRoundEntity
        {
            var record = (await collection
                .FindAsync(record => record.Number == gameRoundId))
                .FirstOrDefault();

            return record;
        }

#pragma warning disable S1067 // Expressions should not be too complex

        private async Task<T> FindTicket<T>(
                IMongoCollection<T> collection,
                int obCustId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity
        {
            var record = (await collection
                .FindAsync(record => record.MemberId == obCustId
                                    && record.SiteId == siteId
                                    && record.Round == gameRoundId
                                    && (!freeBet || record.FreeBet == freeBet)))
                .FirstOrDefault();

            return record;
        }

        private async Task<IList<T>> FindTicketList<T>(
                IMongoCollection<T> collection,
                int obCustId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity
        {
            var record = (await collection
                .FindAsync(record => record.MemberId == obCustId
                                    && record.SiteId == siteId
                                    && record.Round == gameRoundId
                                    && (!freeBet || record.FreeBet == freeBet)))
                .ToList();

            return record;
        }

        private async Task<bool> RemoveTicket<T>(
                IMongoCollection<T> collection,
                int obCustId,
                string siteId,
                long gameRoundId,
                bool freeBet)
            where T : BaseBetEntity
        {
            var result = (await collection
                .DeleteManyAsync(record => record.MemberId == obCustId
                                    && record.SiteId == siteId
                                    && record.Round == gameRoundId
                                    && (!freeBet || record.FreeBet == freeBet)));

            return result.DeletedCount > 0;
        }

        private async Task<bool> RemoveRound<T>(IMongoCollection<T> collection, long gameRoundId)
            where T : BaseRoundEntity
        {
            var result = (await collection.DeleteManyAsync(record => record.Number == gameRoundId));

            return result.DeletedCount > 0;
        }

#pragma warning restore S1067 // Expressions should not be too complex

        private GameTicketCollection GetCollections(byte gameTypeId)
        {
            var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

            return gameCollections.ContainsKey(gameId) ? gameCollections[gameId] : default;
        }
    }
}