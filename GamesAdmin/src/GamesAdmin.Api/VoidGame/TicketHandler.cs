using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.Entities;
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
    }

    public class TicketHandler : ITicketHandler
    {
        private const string ArchivePostfix = "_bk";
        private const string RemovePostfix = "_rm";

        private static Dictionary<GameId, GameTicketCollection> gameCollections = new Dictionary<GameId, GameTicketCollection>
        {
            {GameId.BigSmall, new GameTicketCollection("taixiu_phiens", "taixiu_ones", "taixiu_cuocs") }
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

#pragma warning disable S1067 // Expressions should not be too complex

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

        private async Task<List<T>> FindTicketList<T>(
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

#pragma warning restore S1067 // Expressions should not be too complex

        private GameTicketCollection GetCollections(byte gameTypeId)
        {
            var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

            return gameCollections.ContainsKey(gameId) ? gameCollections[gameId] : default;
        }
    }
}