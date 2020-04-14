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

        Task<IEnumerable<BetInfo>> GetCurrentBetAmount();
    }

    public class GameRoundService : IGameRoundService
    {
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<BigSmallRoundEntity> repository;
        private readonly IGenericRepository<BigSmallOnesEntity> betRepository;

        public GameRoundService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<BigSmallRoundEntity> repository,
            IGenericRepository<BigSmallOnesEntity> betRepository)
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
     
        public async Task<IEnumerable<BetInfo>> GetCurrentBetAmount()
        {
            var latestRound = await GetLastestRound();
            var currentRound = latestRound.Number + 1;

            var betEntities = betRepository.FilterBy(entity => entity.Round == currentRound).ToList();

            return betEntities.Select(entity => mapper.Map<BetInfo>(entity));
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
