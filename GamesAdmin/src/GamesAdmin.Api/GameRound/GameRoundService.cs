using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameRound
{
    public interface IGameRoundService 
    {
        Task<IEnumerable<Round>> Get();

        Task<int> GetLastestRound();
    }

    public class GameRoundService : IGameRoundService
    {
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<MiniSicboRoundEntity> repository;

        public GameRoundService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<MiniSicboRoundEntity> repository)
        {
            this.repository = repository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Round>> Get()
        {
            try
            {
                //should be used for multiple games
                var gameEntities = repository.AsQueryable().ToList();

                return gameEntities
                    .Select(entity => mapper.Map<Round>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return await Task.FromResult(Enumerable.Empty<Round>());
            }
        }

        public Task<int> GetLastestRound()
        {
            throw new NotImplementedException();
        }
    }
}
