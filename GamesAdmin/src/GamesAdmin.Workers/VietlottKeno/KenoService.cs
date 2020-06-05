using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.VietlottKeno
{
    public interface IKenoService
    {
        Task AddNewRoundResultAsync(VietLottKenoRound kenoRound);

        Task AddMissedRoundsAsync(IEnumerable<VietLottKenoRound> kenoRounds);
    }

    public class KenoService : IKenoService
    {
        private readonly IKenoRepository repository;

        public KenoService(IKenoRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddNewRoundResultAsync(VietLottKenoRound kenoRound)
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
        }

        public async Task AddMissedRoundsAsync(IEnumerable<VietLottKenoRound> kenoRounds)
        {
            var roundIds = kenoRounds.Select(x => long.Parse(x.DrawId));
            var latestRound = await repository.GetLatestRoundAsync();

            if (latestRound != null && latestRound.Result == null && kenoRounds.Any(x => long.Parse(x.DrawId) == latestRound.Number))
            {
                var updatedLatestRound = CreateEntity(kenoRounds.FirstOrDefault(x => long.Parse(x.DrawId) == latestRound.Number));

                await repository.UpdateAsync(updatedLatestRound);
            }

            var missedRounds = kenoRounds.Where(x => latestRound == null || latestRound.Number < long.Parse(x.DrawId))
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