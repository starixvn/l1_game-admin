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

        Task<bool> UpdateSiteExclusions(string name, string[] exclusions);

        Task<bool> UpdateSuperPlayers(string[] superPlayers);
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

        public async Task<bool> UpdateSiteExclusions(string name, string[] exclusions)
        {
            try
            {
                var gameEnum = Enumeration.FromValue<Game>(name);
                var configName = ((int)gameEnum.GameId).ToString();

                return await UpdateConfigs(GlobalConfigType.SiteExclusion, configName, exclusions, CreateDefaultSiteExclusions);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateSuperPlayers(string[] superPlayers)
            => await UpdateConfigs(GlobalConfigType.SuperPlayers, "names", superPlayers);

        private async Task<GlobalConfigEntity> GetOrCreateConfigs(
            string configType,
            Func<Dictionary<string, IList<string>>> createConfigsFunc = null)
        {
            try
            {
                var globalConfig = await globalConfigRepository.FindOneAsync(config => config.Type == configType);

                if (globalConfig != null)
                {
                    return globalConfig;
                }

                var newConfig = new GlobalConfigEntity
                {
                    Type = configType,
                    Configs = (createConfigsFunc ?? CreateDefaultConfigs)()
                };

                await globalConfigRepository.InsertOneAsync(newConfig);

                return newConfig;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }

        private Dictionary<string, IList<string>> CreateDefaultSiteExclusions()
        {
            var siteExclusions = new Dictionary<string, IList<string>>();
            var gameIds = Enum.GetValues(typeof(GameId)).Cast<GameId>();

            foreach (var id in gameIds)
            {
                siteExclusions.Add(((int)id).ToString(), new List<string>());
            }

            return siteExclusions;
        }

        private Dictionary<string, IList<string>> CreateDefaultConfigs()
            => new Dictionary<string, IList<string>>();

        private async Task<bool> UpdateConfigs(
            GlobalConfigType type,
            string configsName,
            string[] configValue,
            Func<Dictionary<string, IList<string>>> createConfigsFunc = null)
        {
            try
            {
                var globalConfig = await GetOrCreateConfigs(type.Value, createConfigsFunc);

                if (globalConfig == null)
                {
                    throw new InvalidOperationException($"Cannot find {type.DisplayName}.");
                }

                globalConfig.Configs[configsName] = configValue;
                var update = Builders<GlobalConfigEntity>.Update
                    .Set(field => field.Configs, globalConfig.Configs)
                    .Set(field => field.Time, DateTime.UtcNow);

                await globalConfigRepository.UpdateAsync(config => config.Type == type.Value, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }
    }
}