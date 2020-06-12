using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using GamesAdmin.Workers._Shared;
using MongoDB.Driver;
using Refit;
using Sentry;

namespace GamesAdmin.VietlottKeno
{
    public interface IKenoService
    {
        Task<ProVietKenoRound> AddNewRoundResultAsync(VietLottKenoRoundResult kenoRound);

        Task<IEnumerable<ProVietKenoRound>> AddMissedRoundsAsync(IEnumerable<VietLottKenoRoundResult> kenoRounds);

        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);
    }

    public class KenoService : IKenoService
    {
        private readonly IKenoRepository repository;
        private readonly IGameAdminApi gameAdminApi;


        public KenoService(IKenoRepository repository, IGameAdminApi gameAdminApi)
        {
            this.repository = repository;
            this.gameAdminApi = gameAdminApi;
        }

        public async Task<ProVietKenoRound> AddNewRoundResultAsync(VietLottKenoRoundResult kenoRound)
        {
            var roundId = long.Parse(kenoRound.DrawId);
            var entity = CreateEntity(kenoRound);
            await repository.UpdateAsync(entity,
                new UpdateOptions
                {
                    IsUpsert = true
                });

            var nextRoundEntity = new ProVietKenoRoundEntity
            {
                Number = roundId + 1,
                Time = CalculateNextRoundTime(entity.Time)
            };

            await repository.InsertOneAsync(nextRoundEntity);

            return ConvertToModel(entity);
        }


        public async Task<IEnumerable<ProVietKenoRound>> AddMissedRoundsAsync(IEnumerable<VietLottKenoRoundResult> kenoRounds)
        {
            var roundIds = kenoRounds.Select(x => long.Parse(x.DrawId));
            var latestRound = await repository.GetLatestRoundAsync();

            if (latestRound != null)
            {
                var isLatestRoundResultExisted = kenoRounds.Any(x => long.Parse(x.DrawId) == latestRound.Number);

                if (latestRound.Result == null && isLatestRoundResultExisted)
                {
                    var updatedLatestRound = CreateEntity(kenoRounds.FirstOrDefault(x => long.Parse(x.DrawId) == latestRound.Number));
                    updatedLatestRound.Time = latestRound.Time;
                    await repository.UpdateAsync(updatedLatestRound);
                }

                var missedRounds = kenoRounds.Where(x => latestRound.Number < long.Parse(x.DrawId))
                    .Select(x => CreateEntity(x))
                    .OrderBy(x => x.Number);

                if (missedRounds.Any())
                {
                    var time = CalculateNextRoundTime(latestRound.Time, missedRounds.First().Number - latestRound.Number);

                    foreach (var round in missedRounds)
                    {
                        round.Time = time;
                        time = CalculateNextRoundTime(time);
                    }

                    await repository.InsertManyAsync(
                        new List<ProVietKenoRoundEntity>(missedRounds)
                        {
                            new ProVietKenoRoundEntity {
                                Number = missedRounds.LastOrDefault().Number + 1,
                                Time = time
                            }
                        });

                    return missedRounds.Select(ConvertToModel);
                }
                else
                {
                    if (isLatestRoundResultExisted)
                    {
                        var nextRoundEntity = new ProVietKenoRoundEntity
                        {
                            Number = latestRound.Number + 1,
                            Time = CalculateNextRoundTime(latestRound.Time)
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

        private DateTime CalculateNextRoundTime(DateTime time, long roundDistance = 1)
        {
            var startTimeOfDay = new TimeSpan(23, 0, 0);
            var endTimeOfDay = new TimeSpan(14, 55, 0);
            var nextRoundTime = time;

            for (var i = 1; i <= roundDistance; i++)
            {
                nextRoundTime = nextRoundTime.AddMinutes(10);
                if (nextRoundTime.TimeOfDay > endTimeOfDay && nextRoundTime.TimeOfDay < startTimeOfDay)
                {
                    nextRoundTime = new DateTime(time.Year, time.Month, time.Day, 23, 1, 0, DateTimeKind.Utc);
                }
            }

            return nextRoundTime;
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
                Big = entity.SettlementResult.Big,
                Even = entity.SettlementResult.Even,
                Earth = entity.SettlementResult.Earth,
                Fire = entity.SettlementResult.Fire,
                Gold = entity.SettlementResult.Gold,
                Wood = entity.SettlementResult.Wood,
                Water = entity.SettlementResult.Water
            };
        }

        private ProVietKenoRoundEntity CreateEntity(VietLottKenoRoundResult kenoRound)
        {
            var result = kenoRound.Data.Split("|").Select(x => byte.Parse(x)).OrderBy(x => x);
            var sum = result.Sum(x => x);

            var entity = new ProVietKenoRoundEntity
            {
                Time = DateTime.UtcNow,
                Number = long.Parse(kenoRound.DrawId),
                Result = result,
                SettlementResult = new SettlementResult()
            };

            if (sum >= 811)
            {
                entity.SettlementResult.Big = true;
            }
            else
            {
                entity.SettlementResult.Small = true;
            }

            if (sum % 2 == 0)
            {
                entity.SettlementResult.Even = true;
            }
            else
            {
                entity.SettlementResult.Odd = true;
            }

            if (sum >= 210 && sum <= 695)
            {
                entity.SettlementResult.Gold = true;
            }

            if (sum >= 696 && sum <= 763)
            {
                entity.SettlementResult.Wood = true;
            }

            if (sum >= 764 && sum <= 855)
            {
                entity.SettlementResult.Water = true;
            }

            if (sum >= 856 && sum <= 923)
            {
                entity.SettlementResult.Fire = true;
            }

            if (sum >= 924 && sum <= 1410)
            {
                entity.SettlementResult.Earth = true;
            }

            return entity;
        }
    }
}