using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.VietlottKeno
{
    public interface IKenoService
    {
        Task<VietlottKenoRound> AddNewRoundResultAsync(VietLottKenoRoundResult kenoRound);

        Task<IEnumerable<VietlottKenoRound>> AddMissedRoundsAsync(IEnumerable<VietLottKenoRoundResult> kenoRounds);
    }

    public class KenoService : IKenoService
    {
        private readonly IKenoRepository repository;

        public KenoService(IKenoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<VietlottKenoRound> AddNewRoundResultAsync(VietLottKenoRoundResult kenoRound)
        {
            var roundId = long.Parse(kenoRound.DrawId);
            var entity = CreateEntity(kenoRound);
            await repository.UpdateAsync(entity,
                new UpdateOptions
                {
                    IsUpsert = true
                });

            var nextRoundEntity = new VietlottKenoRoundEntity
            {
                Number = roundId + 1,
                Time = entity.Time.AddMinutes(10)
            };

            await repository.InsertOneAsync(nextRoundEntity);

            return ConvertToModel(entity);
        }

        public async Task<IEnumerable<VietlottKenoRound>> AddMissedRoundsAsync(IEnumerable<VietLottKenoRoundResult> kenoRounds)
        {
            var roundIds = kenoRounds.Select(x => long.Parse(x.DrawId));
            var latestRound = await repository.GetLatestRoundAsync();

            if (latestRound != null)
            {
                if (latestRound.Result == null && kenoRounds.Any(x => long.Parse(x.DrawId) == latestRound.Number))
                {
                    var updatedLatestRound = CreateEntity(kenoRounds.FirstOrDefault(x => long.Parse(x.DrawId) == latestRound.Number));
                    updatedLatestRound.Time = latestRound.Time;
                    await repository.UpdateAsync(updatedLatestRound);
                }

                var missedRounds = kenoRounds.Where(x => latestRound == null || latestRound.Number < long.Parse(x.DrawId))
                    .Select(x => CreateEntity(x))
                    .OrderBy(x => x.Number);

                if (missedRounds.Any())
                {
                    var time = latestRound.Time.AddMinutes(10);

                    foreach (var round in missedRounds)
                    {
                        round.Time = time;
                        time = time.AddMinutes(10);
                    }

                    await repository.InsertManyAsync(missedRounds);

                    return missedRounds.Select(ConvertToModel);
                }
            }

            return Enumerable.Empty<VietlottKenoRound>();
        }

        private VietlottKenoRound ConvertToModel(VietlottKenoRoundEntity entity)
        {
            return new VietlottKenoRound
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

        private VietlottKenoRoundEntity CreateEntity(VietLottKenoRoundResult kenoRound)
        {
            var result = kenoRound.Data.Split("|").Select(x => byte.Parse(x)).OrderBy(x => x);
            var sum = result.Sum(x => x);

            var entity = new VietlottKenoRoundEntity
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