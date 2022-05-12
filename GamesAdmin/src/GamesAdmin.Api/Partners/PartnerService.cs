using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.Partners
{
    public interface IPartnerService
    {
        Task<bool> Create(Partner partner);

        Task<IEnumerable<Partner>> GetAll();

        Task<bool> Update(Partner partner);

        Task<bool> Delete(string partnerId);

        Task<Partner> GetById(string partnerId);

        Task<PartnerConfig> GetConfig(string partnerId, string type);

        Task<bool> UpdateSiteExclusion(string partnerId, int gameId, string[] exclusionSites);

        Task<bool> UpdateSuperPlayers(string partnerId, string[] superPlayers);
    }

    public class PartnerService : IPartnerService
    {
        private readonly IGenericRepository<PartnerEntity> partnerRepository;
        private readonly IGenericRepository<PartnerConfigEntity> partnerConfigRepository;
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;

        public PartnerService(
            ISentryClient sentryClient,
            IMapper mapper,
            IGenericRepository<PartnerEntity> partnerRepository,
            IGenericRepository<PartnerConfigEntity> partnerConfigRepository)
        {
            this.sentryClient = sentryClient;
            this.partnerRepository = partnerRepository;
            this.mapper = mapper;
            this.partnerConfigRepository = partnerConfigRepository;
        }

        public async Task<bool> Create(Partner partner)
        {
            try
            {
                var existPartner = await partnerRepository.FindOneAsync(entity => entity.PartnerId == partner.PartnerId);

                if (existPartner != null)
                {
                    return false;
                }

                var entity = mapper.Map<PartnerEntity>(partner);

                await partnerRepository.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }

            return true;
        }

        public async Task<bool> Delete(string partnerId)
        {
            try
            {
                await partnerRepository.DeleteOneAsync(entity => entity.PartnerId == partnerId);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public Task<IEnumerable<Partner>> GetAll()
        => Task.Run(() =>
        {
            try
            {
                var partners = partnerRepository.AsQueryable().ToList();

                return partners.Select(entity => mapper.Map<Partner>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<Partner>();
            }
        });

        public async Task<bool> Update(Partner partner)
        {
            var userEntity = await partnerRepository.FindOneAsync(entity => entity.PartnerId.Equals(partner.PartnerId));

            if (userEntity != null)
            {
                var update = Builders<PartnerEntity>.Update.Set(field => field.Name, partner.Name)
                    .Set(field => field.SecretKey, partner.SecretKey)
                    .Set(field => field.MobiVersionUrl, partner.MobiVersionUrl)
                    .Set(field => field.ClubVersionUrl, partner.ClubVersionUrl)
                    .Set(field => field.MiniVersionUrl, partner.MiniVersionUrl)
                    .Set(field => field.TestCurrencyUrl, partner.TestCurrencyUrl);


                await partnerRepository.UpdateAsync(user => user.PartnerId == partner.PartnerId, update);

                return true;
            }

            return false;

        }

        public async Task<Partner> GetById(string partnerId)
        {
            try
            {
                var partnerEntity = await partnerRepository.FindOneAsync(entity => entity.PartnerId == partnerId);
                return mapper.Map<Partner>(partnerEntity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return null;
            }
        }

        public async Task<PartnerConfig> GetConfig(string partnerId, string type)
        {
            try
            {
                var config = await partnerConfigRepository.FindOneAsync(entity => entity.PartnerId == partnerId && entity.Type == type);

                return mapper.Map<PartnerConfig>(config);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<bool> UpdateSiteExclusion(string partnerId, int gameId, string[] exclusionSites)
        {
            try
            {
                var configType = PartnerConfigType.SiteExclusion.Value;
                var oldConfig = await GetConfig(partnerId, configType);
                var game = Game.FromGameId((GameId)gameId);
                var configKey = gameId.ToString();

                if (oldConfig == null)
                {
                    var newValues = new Dictionary<string, IList<string>>() { { configKey, exclusionSites.ToList() } };
                    oldConfig = new PartnerConfig(partnerId, configType, newValues);
                    await partnerConfigRepository.InsertOneAsync(mapper.Map<PartnerConfigEntity>(oldConfig));
                }
                else
                {
                    sentryClient.CaptureChangedSetting(
                        game.Value,
                        "Site Exclusion",
                        string.Empty,
                        new SettingChangedModel(string.Join(", ", oldConfig.Value[configKey] ?? new List<string>()), string.Join(", ", exclusionSites)));

                    oldConfig.Value[configKey] = exclusionSites.ToList();
                    var update = Builders<PartnerConfigEntity>.Update.Set(entity => entity.Value, oldConfig.Value);
                    await partnerConfigRepository.UpdateAsync(entity => entity.PartnerId == oldConfig.PartnerId && entity.Type == oldConfig.Type, update);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        public async Task<bool> UpdateSuperPlayers(string partnerId, string[] superPlayers)
        {
            try
            {
                var configType = PartnerConfigType.SuperPlayers.Value;
                var oldConfig = await GetConfig(partnerId, configType);
                var configKey = PartnerConfigKey.Name.Value;

                if (oldConfig == null)
                {
                    var newValues = new Dictionary<string, IList<string>>() { { configKey, superPlayers.ToList() } };
                    oldConfig = new PartnerConfig(partnerId, configType, newValues);
                    await partnerConfigRepository.InsertOneAsync(mapper.Map<PartnerConfigEntity>(oldConfig));
                }
                else
                {
                    oldConfig.Value[configKey] = superPlayers.ToList();
                    var update = Builders<PartnerConfigEntity>.Update.Set(entity => entity.Value, oldConfig.Value);
                    await partnerConfigRepository.UpdateAsync(entity => entity.PartnerId == oldConfig.PartnerId && entity.Type == oldConfig.Type, update);
                }

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
