using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.VoidGame
{
    public interface IRoundHandler
    {
        Task<bool> Void(long gameRoundId, byte gameTypeId);

        Task<bool> Exist(long gameRoundId, byte gameTypeId);
    }

    public class RoundHandler : IRoundHandler
    {
        private const string ArchivePostfix = "_bk";
        private const string RemovePostfix = "_rm";

        private readonly ISentryClient sentryClient;
        private readonly IMongoDatabase activeDatabase;
        private readonly IMongoDatabase archiveDatabase;
        private readonly IMongoDatabase removedDatabase;

        public static Dictionary<GameId, string> RoundCollections = new Dictionary<GameId, string>
        {
            [GameId.NightClub] = "nightclub_rounds",
        };

        public RoundHandler(IAppSettings appSettings, ISentryClient sentryClient)
        {
            this.sentryClient = sentryClient;

            var activeClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            activeDatabase = activeClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName);

            var archiveClient = new MongoClient(appSettings.ArchiveDatabaseSettings.ConnectionString);
            archiveDatabase = archiveClient.GetDatabase(appSettings.ArchiveDatabaseSettings.DatabaseName);

            var removedClient = new MongoClient(appSettings.RemovedDatabaseSettings.ConnectionString);
            removedDatabase = removedClient.GetDatabase(appSettings.RemovedDatabaseSettings.DatabaseName);
        }

        public static string GetCollections(byte gameTypeId)
        {
            var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

            return RoundCollections.ContainsKey(gameId) ? RoundCollections[gameId] : default;
        }

        public async Task<bool> Void(long gameRoundId, byte gameTypeId)
        {
            try
            {
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

                return gameId switch
                {
                    GameId.NightClub => await Move<NightClubRoundEntity>(gameRoundId, gameTypeId),
                    _ => false,
                };
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<bool> Exist(long gameRoundId, byte gameTypeId)
        {
            try
            {
                var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

                var gameTicket = gameId switch
                {
                    GameId.NightClub => await Find<NightClubRoundEntity>(gameRoundId, gameTypeId),
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

        private async Task<bool> Move<Round>(long gameRoundId, byte gameTypeId)
           where Round : BaseRoundEntity
        {
            try
            {
                var collectionName = GetCollections(gameTypeId);
                var gameTicket = await Find<Round>(gameRoundId, gameTypeId);

                if (gameTicket.Records.Count > 0)
                {
                    var collection = removedDatabase.GetCollection<Round>($"{collectionName}{RemovePostfix}");
                    await collection.InsertManyAsync(gameTicket.Records);
                }

                await Remove<Round>(gameRoundId, gameTicket.IsFromArchive, collectionName);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        private async Task<GameRoundRecord<Round>> Find<Round>(long gameRoundId, byte gameTypeId)
           where Round : BaseRoundEntity
        {
            var collectionName = GetCollections(gameTypeId);

            var gameTickets = await FindInActiveOrArchive<Round>(collectionName, gameRoundId);

            if (gameTickets == default)
            {
                gameTickets = await FindInActiveOrArchive<Round>(collectionName, gameRoundId, true);
            }

            return gameTickets;
        }

        private async Task<GameRoundRecord<Round>> FindInActiveOrArchive<Round>(
            string collectionName,
            long gameRoundId,
            bool isArchive = false)
            where Round : BaseRoundEntity
        {
            var database = isArchive ? archiveDatabase : activeDatabase;
            var postfix = isArchive ? ArchivePostfix : string.Empty;

            var collection = database.GetCollection<Round>($"{collectionName}{postfix}");
            var records = await FindRecords(collection, gameRoundId);

            return new GameRoundRecord<Round>(records, isArchive);
        }

        private static async Task<IList<T>> FindRecords<T>(IMongoCollection<T> collection, long gameRoundId)
            where T : BaseRoundEntity
        {
            var record = (await collection.FindAsync(record => record.Number == gameRoundId)).ToList();

            return record;
        }

        private async Task<bool> Remove<Round>(
            long gameRoundId,
            bool isFromArchive,
            string collectionName)
            where Round : BaseRoundEntity
        {
            var database = isFromArchive ? archiveDatabase : activeDatabase;
            var postfix = isFromArchive ? ArchivePostfix : string.Empty;

            if (!string.IsNullOrWhiteSpace(collectionName))
            {
                var collection = database.GetCollection<Round>($"{collectionName}{postfix}");
                await RemoveRecord(collection, gameRoundId);
            }

            return false;
        }

        private static async Task<bool> RemoveRecord<T>(
            IMongoCollection<T> collection,
            long gameRoundId)
            where T : BaseRoundEntity
        {
            var result = await collection.DeleteManyAsync(record => record.Number == gameRoundId);

            return result.DeletedCount > 0;
        }
    }
}