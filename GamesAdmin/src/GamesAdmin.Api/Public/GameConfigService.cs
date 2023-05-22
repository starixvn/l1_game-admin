using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Public.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Public
{
    public interface IGameConfigService
    {
        Task<GameConfig> GetById(int gameId);
    }

    public class GameConfigService : IGameConfigService
    {
        private readonly IMapper mapper;
        private readonly IGenericRepository<GameConfigEntity> repository;

        public GameConfigService(
            IMapper mapper,
            IGenericRepository<GameConfigEntity> repository
            )
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<GameConfig> GetById(int gameId)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.GameId == gameId);

            return gameEntity != null ? mapper.Map<GameConfig>(gameEntity) : default;
        }
    }
}