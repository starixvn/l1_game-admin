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
        Task<bool> UpdateSiteExclusions(string name, string[] exclusions);

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

        public async Task<bool> UpdateSiteExclusions(string name, string[] exclusions)
        {
            try
            {
                var gameEnum = Enumeration.FromValue<Game>(name);
                var globalConfig = await GetOrCreateSiteExclusions();

                if (globalConfig == null)
                {
                    throw new InvalidOperationException($"Cannot find {GlobalConfigType.SiteExclusion.DisplayName}.");
                }

                globalConfig.Configs[((int)gameEnum.GameId).ToString()] = exclusions;

                var update = Builders<GlobalConfigEntity>.Update
                    .Set(field => field.Configs, globalConfig.Configs)
                    .Set(field => field.Time, DateTime.UtcNow);

                await globalConfigRepository.UpdateAsync(config => config.Type == GlobalConfigType.SiteExclusion.Value, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        private async Task<GlobalConfigEntity> GetOrCreateSiteExclusions()
        {
            try
            {
                var globalConfig = await globalConfigRepository.FindOneAsync(config => config.Type == GlobalConfigType.SiteExclusion.Value);

                if (globalConfig == null)
                {
                    var siteExclusions = new Dictionary<string, IList<string>>();
                    var gameIds = Enum.GetValues(typeof(GameId)).Cast<GameId>();

                    foreach (var id in gameIds)
                    {
                        siteExclusions.Add(((int)id).ToString(), new List<string>());
                    }

                    var newConfig = new GlobalConfigEntity { Type = GlobalConfigType.SiteExclusion.Value, Configs = siteExclusions };

                    await globalConfigRepository.InsertOneAsync(newConfig);

                    return newConfig;
                }

                return globalConfig;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
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