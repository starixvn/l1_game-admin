using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.ProVietKeno;
using GamesAdmin.Workers._Shared;
using GamesAdmin.Workers.VietlottKeno;
using MongoDB.Driver;
using Refit;
using Sentry;

namespace GamesAdmin.VietlottKeno
{
    public interface IKenoService
    {
        Task<ProVietKenoRound> AddNewRoundResultAsync(CreateProVietKenoRound kenoRound);

        Task<IEnumerable<ProVietKenoRound>> AddMissedRoundsAsync(IEnumerable<CreateProVietKenoRound> kenoRounds);

        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);

        Task<ProVietKenoRound> GetLatestRoundAsync();

        Task<ProVietKenoRound> GetByRoundIdAsync(long roundId);

        Task AddRoundLogEntity(long roundId, DateTime time, string source);
    }

    public class KenoService : IKenoService
    {
        private readonly IKenoRepository repository;
        private readonly IGenericRepository<ProvietKenoRoundLogEntity> logRepository;
        private readonly IGameAdminApi gameAdminApi;

        public KenoService(IKenoRepository repository, IGameAdminApi gameAdminApi, IGenericRepository<ProvietKenoRoundLogEntity> logRepository)
        {
            this.repository = repository;
            this.gameAdminApi = gameAdminApi;
            this.logRepository = logRepository;
        }

        public async Task<ProVietKenoRound> AddNewRoundResultAsync(CreateProVietKenoRound kenoRound)
        {
            var entity = CreateEntity(kenoRound);
            await repository.UpdateAsync(entity,
                new UpdateOptions
                {
                    IsUpsert = true
                });

            var nextRoundEntity = new ProVietKenoRoundEntity
            {
                Number = kenoRound.Number + 1,
                Time = entity.Time.CalculateNextRoundTime()
            };

            await repository.InsertOneAsync(nextRoundEntity);

            return ConvertToModel(entity);
        }

        public async Task<IEnumerable<ProVietKenoRound>> AddMissedRoundsAsync(IEnumerable<CreateProVietKenoRound> kenoRounds)
        {
            var roundIds = kenoRounds.Select(x => x.Number);
            var latestRound = await repository.GetLatestRoundAsync();

            if (latestRound != null)
            {
                var isLatestRoundResultExisted = kenoRounds.Any(x => x.Number == latestRound.Number);

                if (latestRound.Result == null && isLatestRoundResultExisted)
                {
                    var updatedLatestRound = CreateEntity(kenoRounds.FirstOrDefault(x => x.Number == latestRound.Number));
                    updatedLatestRound.Time = latestRound.Time;
                    await repository.UpdateAsync(updatedLatestRound);
                }

                var missedRounds = kenoRounds.Where(x => latestRound.Number < x.Number)
                    .Select(x => CreateEntity(x))
                    .OrderBy(x => x.Number)
                    .ToList();

                if (missedRounds.Any())
                {
                    var time = latestRound.Time.CalculateNextRoundTime(missedRounds.First().Number - latestRound.Number);

                    foreach (var round in missedRounds)
                    {
                        round.Time = time;
                        time = time.CalculateNextRoundTime();
                    }

                    await repository.InsertManyAsync(new List<ProVietKenoRoundEntity>(missedRounds)
                    {
                          new ProVietKenoRoundEntity
                        {
                            Number = missedRounds.LastOrDefault().Number + 1,
                            Time = time
                        }
                    });

                    return missedRounds.Select(x => ConvertToModel(x));
                }
                else
                {
                    if (isLatestRoundResultExisted)
                    {
                        var nextRoundEntity = new ProVietKenoRoundEntity
                        {
                            Number = latestRound.Number + 1,
                            Time = latestRound.Time.CalculateNextRoundTime()
                        };

                        await repository.InsertOneAsync(nextRoundEntity);
                    }
                }
            }

            return Enumerable.Empty<ProVietKenoRound>();
        }

        public async Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds)
        {
            try
            {
                await gameAdminApi.NotifyRoundResult(kenoRounds);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex is ApiException)
            {
                var apiEx = ex as ApiException;

                if (apiEx != null)
                {
                    SentrySdk.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}");
                }
            }

            SentrySdk.CaptureException(ex);
        }

        private ProVietKenoRound ConvertToModel(ProVietKenoRoundEntity entity)
        {
            return new ProVietKenoRound
            {
                Time = entity.Time,
                Result = entity.Result,
                Number = entity.Number,
                Big = entity.SettlementResult?.Big,
                Small = entity.SettlementResult?.Small,
                Even = entity.SettlementResult?.Even,
                Odd = entity.SettlementResult?.Odd,
                Earth = entity.SettlementResult?.Earth,
                Fire = entity.SettlementResult?.Fire,
                Gold = entity.SettlementResult?.Gold,
                Wood = entity.SettlementResult?.Wood,
                Water = entity.SettlementResult?.Water
            };
        }

        private ProVietKenoRoundEntity CreateEntity(CreateProVietKenoRound kenoRound)
        {
            var sum = kenoRound.Result.Sum(x => x);

            var entity = new ProVietKenoRoundEntity
            {
                Time = kenoRound.Time,
                Number = kenoRound.Number,
                Result = kenoRound.Result,
                SettlementResult = new SettlementResult(sum)
            };

            return entity;
        }

        public async Task<ProVietKenoRound> GetLatestRoundAsync()
        {
            var entity = await repository.GetLatestRoundAsync();

            return entity != null ? ConvertToModel(entity) : null;
        }

        public async Task<ProVietKenoRound> GetByRoundIdAsync(long roundId)
        {
            var entity = await repository.FindOneAsync(x => x.Number == roundId);

            return entity != null ? ConvertToModel(entity) : null;
        }

        public async Task AddRoundLogEntity(long roundId, DateTime time, string source)
        {
            await logRepository.InsertOneAsync(new ProvietKenoRoundLogEntity
            {
                Number = roundId,
                Time = time,
                Source = source
            });
        }
    }
}