using System;
using System.Collections.Generic;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.VoidGame.Models;
using GamesAdmin.Core.Enumeration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Api.VoidGame
{
    public interface ITicketFinder
    {
        BsonDocument Find(int obCustId, string siteId, int gameRoundId, byte gameTypeId, bool freeBet);
    }

    public class TicketFinder : ITicketFinder
    {
        private static Dictionary<GameId, GameTicketCollection> gameCollections = new Dictionary<GameId, GameTicketCollection>
        {
            {GameId.BigSmall, new GameTicketCollection("taixiu_phiens", "taixiu_ones", "taixiu_cuocs") }
        };

        private readonly IAppSettings appSettings;
        private readonly IMongoDatabase activeDatabase;
        private readonly IMongoDatabase archiveDatabase;
        private readonly IMongoDatabase removedDatabase;

        public TicketFinder(IAppSettings appSettings)
        {
            this.appSettings = appSettings;

            var activeClient = new MongoClient(appSettings.DatabaseSettings.ConnectionString);
            activeDatabase = activeClient.GetDatabase(appSettings.DatabaseSettings.DatabaseName);

            var archiveClient = new MongoClient(appSettings.ArchiveDatabaseSettings.ConnectionString);
            archiveDatabase = archiveClient.GetDatabase(appSettings.ArchiveDatabaseSettings.DatabaseName);

            var removedClient = new MongoClient(appSettings.RemovedDatabaseSettings.ConnectionString);
            removedDatabase = removedClient.GetDatabase(appSettings.RemovedDatabaseSettings.DatabaseName);
        }

        public BsonDocument Find(int obCustId, string siteId, int gameRoundId, byte gameTypeId, bool freeBet)
        {
            //TODO map game type id to collections name
            var collections = GetCollections(gameTypeId);
            //TODO first lookup active db
            //TODO lookup archive db

            return default;
        }

        private GameTicketCollection GetCollections(byte gameTypeId)
        {
            var gameId = Enum.Parse<GameId>(gameTypeId.ToString());

            return gameCollections.ContainsKey(gameId) ? gameCollections[gameId] : default;
        }
    }
}