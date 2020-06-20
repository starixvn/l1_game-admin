using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.ProVietKeno.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Api.ProVietKeno
{
    public interface IProVietKenoService
    {
        Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date);

        Task<ProVietKenoRound> CreateAsync(CreateProVietKenoRound kenoRound);

        Task<ProVietKenoRound> GetByRoundId(long roundId);

        Task<IEnumerable<ProVietKenoRound>> GetUnsettledRoundsAsync();
    }

    public class ProVietKenoService : IProVietKenoService
    {
        private IGenericRepository<ProVietKenoRoundEntity> repository;

        public ProVietKenoService(IGenericRepository<ProVietKenoRoundEntity> repository)
        {
            this.repository = repository;
        }

        public async Task<ProVietKenoRound> GetByRoundId(long roundId)
        {
            var entity = await repository.FindOneAsync(x => x.Number == roundId);

            if (entity != null)
            {
                return entity.ConvertToModel();
            }

            return null;
        }

        public async Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date)
        {
            var fromDate = date;
            var toDate = fromDate.AddDays(1);

            var result = await repository.FilterByAsync(x => x.Time >= fromDate && x.Time < toDate);

            var rounds = result.Select(x => x.ConvertToModel());
            return rounds;
        }

        public async Task<ProVietKenoRound> CreateAsync(CreateProVietKenoRound kenoRound)
        {
            var sum = kenoRound.Result.Sum(x => x);

            var entity = new ProVietKenoRoundEntity
            {
                Time = kenoRound.Time,
                Result = kenoRound.Result,
                Number = kenoRound.Number,
                SettlementResult = new SettlementResult(sum)
            };

            var updateDefinition = Builders<ProVietKenoRoundEntity>.Update;

            await repository.UpdateAsync(x => x.Number == entity.Number,
                    updateDefinition.Combine(
                        updateDefinition.Set(x => x.Time, entity.Time),
                        updateDefinition.Set(x => x.Result, entity.Result),
                        updateDefinition.Set(x => x.SettlementResult, entity.SettlementResult)
                    ),
                    new UpdateOptions { IsUpsert = true }
                );

            if (!repository.AsQueryable().Any(x => x.Number > entity.Number))
            {
                await InsertNextEmptyRound(entity.Number, entity.Time);
            }

            return entity.ConvertToModel();
        }

        private async Task InsertNextEmptyRound(long currentRoundId, DateTime currentTime)
        {
            var startTimeOfDay = new TimeSpan(23, 0, 0);
            var endTimeOfDay = new TimeSpan(14, 55, 0);
            var nextRoundTime = currentTime.AddMinutes(10);

            if (nextRoundTime.TimeOfDay > endTimeOfDay && nextRoundTime.TimeOfDay < startTimeOfDay)
            {
                nextRoundTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 23, 10, 0, DateTimeKind.Utc);
            }

            var nextRoundEntity = new ProVietKenoRoundEntity
            {
                Number = currentRoundId + 1,
                Time = nextRoundTime
            };

            await repository.InsertOneAsync(nextRoundEntity);
        }

        public async Task<IEnumerable<ProVietKenoRound>> GetUnsettledRoundsAsync()
        {
            var result = (await repository.FilterByAsync(x => !x.Settled && x.Result != null && x.SettlementResult != null));

            var rounds = result.Select(x => x.ConvertToModel());
            return rounds;
        }
    }
}