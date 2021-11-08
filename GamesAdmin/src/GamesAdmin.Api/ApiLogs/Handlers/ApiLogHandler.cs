using System;
using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.ApiLogs;
using GamesAdmin.Database.ApiLogs.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Api.ApiLogs.Handlers
{
    public abstract class ApiLogHandler
    {
        protected ApiLogHandler(IMapper mapper, IServiceProvider serviceProvider, IAppSettings appSettings)
        {
            ServiceProvider = serviceProvider;
            Mapper = mapper;
            AppSettings = appSettings;
        }

        protected IMapper Mapper { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected IAppSettings AppSettings { get; }

        protected static bool IsSBLog(ApiLogType type) => type == ApiLogType.SignInRequest || type == ApiLogType.RetrieveRequests;

        protected ILogRepository<PartnerLogEntity> GetPartnerRepository(ApiLogType type)
        {
            var database = GetDatabase();
            var collectionName = ApiLogConstant.CollectionMappings[type];
            var collection = database.GetCollection<PartnerLogEntity>(collectionName);
            var repository = (ILogRepository<PartnerLogEntity>)ServiceProvider.GetService(typeof(ILogRepository<PartnerLogEntity>));

            repository.SetCollection(collection);

            return repository;
        }

        protected ILogRepository<SBLogEntity> GetSBRepository(ApiLogType type)
        {
            var database = GetDatabase();
            var collectionName = ApiLogConstant.CollectionMappings[type];
            var collection = database.GetCollection<SBLogEntity>(collectionName);
            var repository = (ILogRepository<SBLogEntity>)ServiceProvider.GetService(typeof(ILogRepository<SBLogEntity>));

            repository.SetCollection(collection);

            return repository;
        }

        protected IMongoDatabase GetDatabase()
        {
            var databaseSettings = AppSettings.ApiLogDatabaseSettings;
            var client = new MongoClient(databaseSettings.ConnectionString);

            return client.GetDatabase(databaseSettings.DatabaseName);
        }
    }
}