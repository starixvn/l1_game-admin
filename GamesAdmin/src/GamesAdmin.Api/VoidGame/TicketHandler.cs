using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<bool> Void(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet);

        Task<bool> Exist(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet);
    }

    public class TicketHandler : ITicketHandler
    {
        private const string ArchivePostfix = "_bk";
        private const string RemovePostfix = "_rm";

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
                    GameId.Blackjack => await Move<BlackjackBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
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

        public async Task<bool> Exist(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
        {
            try
            {
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

                IGameTicket gameTicket = gameId switch
                {
                    GameId.BigSmall => await Find<BigSmallOnesEntity, BigSmallBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.SeDie => await Find<SeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.FishPrawnCrab => await Find<FishPrawnCrabBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.BigSmallTurbo => await Find<BigSmallTurboOnesEntity, BigSmallTurboBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.OddEven => await Find<OddEvenOnesEntity, OddEvenBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.OddEvenTurbo => await Find<OddEvenTurboOnesEntity, OddEvenTurboBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.DragonTiger => await Find<DragonTigerBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoProMax => await Find<KenoProMaxBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.Baccarat => await Find<BaccaratBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.MiniSeDie => await Find<MiniSeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoProViet => await Find<KenoProVietOnesEntity, KenoProVietBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMax => await Find<KenoMaxOnesEntity, KenoMaxBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMax2 => await Find<KenoMax2OnesEntity, KenoMax2BetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMini => await Find<KenoMiniOnesEntity, KenoMiniBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoMini2 => await Find<KenoMini2OnesEntity, KenoMini2BetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoEast => await Find<KenoEastOnesEntity, KenoEastBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoWest => await Find<KenoWestOnesEntity, KenoWestBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoSouth => await Find<KenoSouthOnesEntity, KenoSouthBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.KenoNorth => await Find<KenoNorthOnesEntity, KenoNorthBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.Roulette => await Find<RouletteOnesEntity, RouletteBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipSeDie => await Find<VipSeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.Blackjack => await Find<BlackjackBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.SpecialSeDie => await Find<SpecialSeDieOnesEntity, SpecialSeDieBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.SicBo => await Find<SicBoOnesEntity, SicBoBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipFishPrawnCrab => await Find<VipFishPrawnCrabBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipBaccarat => await Find<VipBaccaratBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    GameId.VipSeDieWar => await Find<VipSeDieWarOnesEntity, VipSeDieWarBetEntity>(obCustId, siteId, gameRoundId, gameTypeId, freeBet),
                    _ => default,
                };

                return gameTicket?.Exist() == true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        private async Task<bool> Move<One, Bet>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            try
            {
                var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId);
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

        private async Task<bool> Move<Bet>(int obCustId, string siteId, long gameRoundId, byte gameTypeId, bool freeBet)
            where Bet : BaseBetEntity
        {
            try
            {
                var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId);
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

        private async Task<bool> Remove<One, Bet>(
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

        private async Task<bool> Remove<Bet>(
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

        private async Task<GameTicketRecord<One, Bet>> Find<One, Bet>(
                int obCustId,
                string siteId,
                long gameRoundId,
                byte gameTypeId,
                bool freeBet)
            where One : BaseBetEntity
            where Bet : BaseBetEntity
        {
            var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId);

            var gameTickets = await FindInActiveOrArchive<One, Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<One, Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet, true);
            }

            return gameTickets;
        }

        private async Task<GameTicketRecord<Bet>> Find<Bet>(
                int obCustId,
                string siteId,
                long gameRoundId,
                byte gameTypeId,
                bool freeBet)
            where Bet : BaseBetEntity
        {
            var gameCollection = TicketCollectionHelper.GetCollections(gameTypeId);

            var gameTickets = await FindInActiveOrArchive<Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<Bet>(gameCollection, obCustId, siteId, gameRoundId, freeBet, true);
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
            var oneRecord = (await FindTicket(oneCollection, obCustId, siteId, gameRoundId, freeBet)).FirstOrDefault();

            var betCollection = database.GetCollection<Bet>($"{collections.Bet}{postfix}");
            var betRecords = await FindTicket(betCollection, obCustId, siteId, gameRoundId, freeBet);

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
            var bets = await FindTicket(betCollection, obCustId, siteId, gameRoundId, freeBet);

            return new GameTicketRecord<Bet>(bets, isArchive);
        }

#pragma warning disable S1067 // Expressions should not be too complex

        private static async Task<IList<T>> FindTicket<T>(
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

        private static async Task<bool> RemoveTicket<T>(
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

#pragma warning restore S1067 // Expressions should not be too complex
    }
}