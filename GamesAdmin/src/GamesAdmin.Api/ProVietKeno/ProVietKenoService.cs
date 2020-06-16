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

            if(entity != null)
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
            var sum = kenoRound.Results.Sum(x => x);

            var entity = new ProVietKenoRoundEntity
            {
                Time = kenoRound.Time,
                Result = kenoRound.Results,
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

            return entity.ConvertToModel();
        }

        public async Task<IEnumerable<ProVietKenoRound>> GetUnsettledRoundsAsync()
        {
            var result = (await repository.FilterByAsync(x => !x.Settled && x.Result != null && x.SettlementResult != null));

            var rounds = result.Select(x => x.ConvertToModel());
            return rounds;
        }
    }
}