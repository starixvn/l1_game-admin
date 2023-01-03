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
        private readonly IGenericRepository<GlobalConfigEntity> globalConfigRepository;

        public GlobalConfigService(
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<GlobalConfigEntity> globalConfigRepository)
        {
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.globalConfigRepository = globalConfigRepository;
        }

        public async Task<GlobalConfig> GetGlobalConfigByType(GlobalConfigType type)
        {
            try
            {
                var globalConfig = await globalConfigRepository.FindOneAsync(config => config.Type == type.Value);

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