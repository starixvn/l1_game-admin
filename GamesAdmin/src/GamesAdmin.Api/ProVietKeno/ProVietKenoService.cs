using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                return new ProVietKenoRound
                {
                    Time = entity.Time,
                    Result = entity.Result,
                    Number = entity.Number,
                    Big = entity.SettlementResult?.Big,
                    Even = entity.SettlementResult?.Even,
                    Earth = entity.SettlementResult?.Earth,
                    Fire = entity.SettlementResult?.Fire,
                    Gold = entity.SettlementResult?.Gold,
                    Wood = entity.SettlementResult?.Wood,
                    Water = entity.SettlementResult?.Water,
                    Settled = entity.Settled
                };
            }

            return null;
        }

        public async Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date)
        {
            var fromDate = date;
            var toDate = fromDate.AddDays(1);

            var result = await repository.FilterByAsync(x => x.Time >= fromDate && x.Time < toDate);

            var rounds = result.Select(x => new ProVietKenoRound
            {
                Time = x.Time,
                Result = x.Result,
                Number = x.Number,
                Big = x.SettlementResult?.Big,
                Even = x.SettlementResult?.Even,
                Earth = x.SettlementResult?.Earth,
                Fire = x.SettlementResult?.Fire,
                Gold = x.SettlementResult?.Gold,
                Wood = x.SettlementResult?.Wood,
                Water = x.SettlementResult?.Water,
                Settled = x.Settled
            });

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
    }
}