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

        Task<Round> GetLastestRound();

        Task<Round> GetBetAmount();
    }

    public class GameRoundService : IGameRoundService
    {
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<MiniSicboRoundEntity> repository;
        private readonly IGenericRepository<MiniSicboOnesEntity> betRepository;

        public GameRoundService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<MiniSicboRoundEntity> repository,
            IGenericRepository<MiniSicboOnesEntity> betRepository)
        {
            this.repository = repository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.betRepository = betRepository;
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

        //should get bet amount of current round
        public async Task<Round> GetBetAmount()
        {
            var latestRound = await GetLastestRound();

            var betEntities = betRepository.FilterBy(entity => entity.phien == latestRound.Number).ToList();

            return latestRound;
        }

        public Task<Round> GetLastestRound()
        => Task.Run(() =>
            {
                try
                {
                    //should be used for multiple games
                    var gameEntities = repository.AsQueryable().ToList();

                    return mapper.Map<Round>(gameEntities.LastOrDefault());
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);

                    return default;
                }
            });
    }
}
