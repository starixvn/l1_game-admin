using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Games
{
    public interface IGameService
    {
        Task<IEnumerable<GameConfig>> Get();

        Task<GameConfig> Get(Guid id);

        Task<bool> Create(GameConfig game);

        Task<bool> Update(GameConfig game);

        Task<bool> Delete(Guid id);
    }

    public class GameService : IGameService
    {
        private readonly IMapper mapper;
        private readonly GenericRepository repository;

        public GameService(
            IMapper mapper,
            GenericRepository repository)
        {
            this.repository = repository;

            this.mapper = mapper;
        }

        public async Task<IEnumerable<GameConfig>> Get()
        {
            try {
                var gameEntities = await repository.GetAllAsync<GameConfigEntity>(config => true);

                return gameEntities
                    .Select(entity => mapper.Map<GameConfig>(entity));
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                return await Task.FromResult(Enumerable.Empty<GameConfig>());
            }
        }

        public async Task<GameConfig> Get(Guid id)
        {
            var gameEntity = await repository.GetOneAsync<GameConfigEntity>(entity => entity.Id == id);

            return mapper.Map<GameConfig>(gameEntity);
        }

        public async Task<bool> Create(GameConfig game)
        {
            try
            {
                await repository.AddOneAsync(mapper.Map<GameConfigEntity>(game));
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;

                return false;
            }

            return true;
        }

        public async Task<bool> Update(GameConfig game)
        {
            try
            {
                await repository.UpdateOneAsync(mapper.Map<GameConfigEntity>(game));

                return true;
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;

                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                await repository.DeleteOneAsync<GameConfigEntity>(entity => entity.Id == id);

                return true;
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;

                return false;
            }
        }
    }
}
