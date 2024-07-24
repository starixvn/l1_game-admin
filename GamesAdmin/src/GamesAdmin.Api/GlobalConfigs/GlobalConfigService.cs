using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.GlobalConfigs
{
    public interface IGlobalConfigService
    {
        Task<GlobalConfig> GetGlobalConfigByType(GlobalConfigType type);
    }

    public class GlobalConfigService : IGlobalConfigService
    {
        private readonly ISentryClient sentryClient;
        private readonly IMapper mapper;
        private readonly IMongoCollection<GlobalConfigEntity> collection;

        public GlobalConfigService(
            ISentryClient sentryClient,
            IMapper mapper,
            IMongoDatabase database)
        {
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.collection = database.GetCollection<GlobalConfigEntity>(Document.GetCollectionName(typeof(GlobalConfigEntity)));
        }

        public async Task<GlobalConfig> GetGlobalConfigByType(GlobalConfigType type)
        {
            try
            {
                var globalConfig = await collection.Find(config => config.Type == type.Value).FirstOrDefaultAsync();

                return mapper.Map<GlobalConfig>(globalConfig);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }
    }
}