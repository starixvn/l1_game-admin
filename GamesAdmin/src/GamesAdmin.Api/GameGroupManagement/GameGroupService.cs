using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.GameGroup;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Api.GameGroupManagement
{
    public interface IGameGroupService
    {
        Task<IEnumerable<GameGroup>> GetAll();

        Task<GameGroup> GetByGameId(int gameId);

        Task<GameGroup> GetByGroupId(string groupId);

        Task<bool> Create(GameGroup gameGroup);

        Task<bool> Update(GameGroup gameGroup);
    }

    public class GameGroupService : IGameGroupService
    {
        private readonly IGenericRepository<GameGroupEntity> gameGroupRepository;
        private readonly IMapper mapper;

        public GameGroupService(IGenericRepository<GameGroupEntity> gameGroupRepository, IMapper mapper)
        {
            this.gameGroupRepository = gameGroupRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Create(GameGroup gameGroup)
        {
            await gameGroupRepository.InsertOneAsync(new GameGroupEntity
            {
                Name = gameGroup.Name,
                GameIds = gameGroup.GameIds,
                Time = DateTime.UtcNow,
            });

            return true;
        }

        public async Task<IEnumerable<GameGroup>> GetAll()
        {
            var gameGroupEntities = await gameGroupRepository.FilterByAsync(x => true);
            return gameGroupEntities.Select(entity => mapper.Map<GameGroup>(entity));
        }

        public async Task<GameGroup> GetByGameId(int gameId)
        {
            var entity = await gameGroupRepository.FindOneAsync(x => x.GameIds.Contains(gameId));
            return mapper.Map<GameGroup>(entity);
        }

        public async Task<GameGroup> GetByGroupId(string groupId)
        {
            var entity = await gameGroupRepository.FindOneAsync(x => x.Id.Equals(ObjectId.Parse(groupId)));
            return mapper.Map<GameGroup>(entity);
        }

        public async Task<bool> Update(GameGroup gameGroup)
        {
            var update = Builders<GameGroupEntity>.Update
                  .Set(field => field.Name, gameGroup.Name)
                  .Set(field => field.GameIds, gameGroup.GameIds);

            await gameGroupRepository.UpdateAsync(entity => entity.Id.Equals(ObjectId.Parse(gameGroup.Id)), update);

            return true;
        }
    }
}