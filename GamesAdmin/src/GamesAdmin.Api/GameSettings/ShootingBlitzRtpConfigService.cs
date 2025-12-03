using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.ShootingBlitz;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.GameSettings
{
    public interface IShootingBlitzRtpConfigService
    {
        Task<IEnumerable<ShootingBlitzRtpConfig>> GetAllAsync();
        Task<ShootingBlitzRtpConfig> GetByIdAsync(string id);
        Task<ShootingBlitzRtpConfig> CreateAsync(ShootingBlitzRtpConfig config);
        Task<bool> UpdateAsync(string id, ShootingBlitzRtpConfig config);
        Task<bool> DeleteAsync(string id);
    }

    public class ShootingBlitzRtpConfigService : IShootingBlitzRtpConfigService
    {
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<ShootingBlitzRtpConfigEntity> repository;

        public ShootingBlitzRtpConfigService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<ShootingBlitzRtpConfigEntity> repository)
        {
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.repository = repository;
        }

        public async Task<IEnumerable<ShootingBlitzRtpConfig>> GetAllAsync()
        {
            try
            {
                var entities = await Task.Run(() => repository.AsQueryable().ToList());
                return entities.Select(entity => mapper.Map<ShootingBlitzRtpConfig>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Enumerable.Empty<ShootingBlitzRtpConfig>();
            }
        }

        public async Task<ShootingBlitzRtpConfig> GetByIdAsync(string id)
        {
            try
            {
                var entity = await repository.FindByIdAsync(id);
                return entity != null ? mapper.Map<ShootingBlitzRtpConfig>(entity) : null;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<ShootingBlitzRtpConfig> CreateAsync(ShootingBlitzRtpConfig config)
        {
            try
            {
                var entity = mapper.Map<ShootingBlitzRtpConfigEntity>(config);
                entity.Time = DateTime.Now;
                await repository.InsertOneAsync(entity);
                return mapper.Map<ShootingBlitzRtpConfig>(entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string id, ShootingBlitzRtpConfig config)
        {
            try
            {
                var update = Builders<ShootingBlitzRtpConfigEntity>.Update
                    .Set(x => x.Rtp, config.Rtp)
                    .Set(x => x.BombCount, mapper.Map<List<BombCountConfigEntity>>(config.BombCount))
                    .Set(x => x.MultiplierSteps, mapper.Map<List<MultiplierStepsConfigEntity>>(config.MultiplierSteps));

                await repository.UpdateAsync(x => x.Id == ObjectId.Parse(id), update);
                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                await repository.DeleteByIdAsync(id);
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

