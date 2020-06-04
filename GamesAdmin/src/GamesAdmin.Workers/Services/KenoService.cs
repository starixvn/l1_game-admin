using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Workers.Services
{
    public interface IKenoService
    {
        Task AddNewRoundResultAsync(VietLottKenoRound kenoRound);

        Task AddMissedRoundsAsync(IEnumerable<VietLottKenoRound> kenoRounds);
    }

    public class KenoService : IKenoService
    {
        private readonly IGenericRepository<VietlottKenoRoundEntity> repository;

        public KenoService(IGenericRepository<VietlottKenoRoundEntity> repository)
        {
            this.repository = repository;
        }

        public async Task AddNewRoundResultAsync(VietLottKenoRound kenoRound)
        {
            var roundId = long.Parse(kenoRound.DrawId);
            var entity = CreateEntity(kenoRound);
            var existedRound = await repository.FindOneAsync(x => x.Number == roundId);

            var nextRoundEntity = new VietlottKenoRoundEntity
            {
                Number = roundId + 1,
                Time = entity.Time.AddMinutes(10)
            };

            if (existedRound != null)
            {
                var updateDefinition = Builders<VietlottKenoRoundEntity>.Update;

                await repository.UpdateAsync(x => x.Id == existedRound.Id,
                    updateDefinition.Combine(
                        updateDefinition.Set(x => x.Time, entity.Time),
                        updateDefinition.Set(x => x.Result, entity.Result),
                        updateDefinition.Set(x => x.SettlementResult, entity.SettlementResult)
                    ));

                await repository.InsertOneAsync(nextRoundEntity);
            }
            else
            {
                await repository.InsertManyAsync(new[] { entity, nextRoundEntity });
            }
        }

        public async Task AddMissedRoundsAsync(IEnumerable<VietLottKenoRound> kenoRounds)
        {
            var roundIds = kenoRounds.Select(x => long.Parse(x.DrawId));
            var existedRounds = (await repository.FilterByAsync(x => roundIds.Contains(x.Number))).ToList();
            var missedRounds = kenoRounds.Where(x => !existedRounds.Any(er => er.Number == long.Parse(x.DrawId)))
                .Select(x => CreateEntity(x))
                .OrderBy(x => x.Number);

            if (missedRounds.Any())
            {
                await repository.InsertManyAsync(missedRounds);
            }
        }

        private VietlottKenoRoundEntity CreateEntity(VietLottKenoRound kenoRound)
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