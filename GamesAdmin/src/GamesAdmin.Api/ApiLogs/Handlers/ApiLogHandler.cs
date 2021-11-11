using System;
using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Database.ApiLogs;
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

        protected ILogRepository<TEntity> GetRepository<TEntity>(ApiLogType type)
        {
            var database = GetDatabase();
            var collectionName = ApiLogConstant.CollectionMappings[type];
            var collection = database.GetCollection<TEntity>(collectionName);
            var repository = (ILogRepository<TEntity>)ServiceProvider.GetService(typeof(ILogRepository<TEntity>));

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