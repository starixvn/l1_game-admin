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
    public interface IShootingBlitzTeamService
    {
        Task<IEnumerable<ShootingBlitzTeam>> GetAllAsync();
        Task<ShootingBlitzTeam> GetByIdAsync(string id);
        Task<ShootingBlitzTeam> CreateAsync(ShootingBlitzTeam team);
        Task<bool> UpdateAsync(string id, ShootingBlitzTeam team);
        Task<bool> DeleteAsync(string id);
        Task<bool> IsTeamIdExistsAsync(int teamId, string excludeId = null);
    }

    public class ShootingBlitzTeamService : IShootingBlitzTeamService
    {
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<ShootingBlitzTeamEntity> repository;

        public ShootingBlitzTeamService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<ShootingBlitzTeamEntity> repository)
        {
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.repository = repository;
        }

        public async Task<IEnumerable<ShootingBlitzTeam>> GetAllAsync()
        {
            try
            {
                var entities = await Task.Run(() => repository.AsQueryable().ToList());
                return entities.Select(entity => mapper.Map<ShootingBlitzTeam>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return Enumerable.Empty<ShootingBlitzTeam>();
            }
        }

        public async Task<ShootingBlitzTeam> GetByIdAsync(string id)
        {
            try
            {
                var entity = await repository.FindByIdAsync(id);
                return entity != null ? mapper.Map<ShootingBlitzTeam>(entity) : null;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<ShootingBlitzTeam> CreateAsync(ShootingBlitzTeam team)
        {
            try
            {
                // Check if teamId already exists
                if (await IsTeamIdExistsAsync(team.TeamId))
                {
                    sentryClient.CaptureMessage($"Team with TeamId {team.TeamId} already exists.");
                    return null;
                }

                var entity = mapper.Map<ShootingBlitzTeamEntity>(team);
                entity.Time = DateTime.Now;
                await repository.InsertOneAsync(entity);
                return mapper.Map<ShootingBlitzTeam>(entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string id, ShootingBlitzTeam team)
        {
            try
            {
                // Check if teamId already exists for another team
                if (await IsTeamIdExistsAsync(team.TeamId, id))
                {
                    sentryClient.CaptureMessage($"Team with TeamId {team.TeamId} already exists.");
                    return false;
                }

                var update = Builders<ShootingBlitzTeamEntity>.Update
                    .Set(x => x.TeamId, team.TeamId)
                    .Set(x => x.Name, team.Name)
                    .Set(x => x.PlayerList, mapper.Map<List<ShootingBlitzPlayerEntity>>(team.PlayerList));

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

        public async Task<bool> IsTeamIdExistsAsync(int teamId, string excludeId = null)
        {
            try
            {
                var existingTeam = await Task.Run(() => repository.AsQueryable()
                    .FirstOrDefault(t => t.TeamId == teamId));

                if (existingTeam == null)
                {
                    return false;
                }

                // If excludeId is provided, check if it's the same record
                if (!string.IsNullOrEmpty(excludeId) && existingTeam.Id.ToString() == excludeId)
                {
                    return false;
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

