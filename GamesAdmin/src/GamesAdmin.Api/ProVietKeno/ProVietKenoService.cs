using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.ProVietKeno
{
    public interface IProVietKenoService
    {
        Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date);

        Task CreateAsync(CreateProVietKenoRound kenoRound);
    }

    public class ProVietKenoService : IProVietKenoService
    {
        private IGenericRepository<ProVietKenoRoundEntity> repository;

        public ProVietKenoService(IGenericRepository<ProVietKenoRoundEntity> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date)
        {
            var fromtDate = date;
            var toDate = fromtDate.AddDays(1);

            var result = await repository.FilterByAsync(x => x.Time >= fromtDate && x.Time < toDate);

            return result.Select(x => new ProVietKenoRound
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
                Water = x.SettlementResult?.Water
            });
        }

        public async Task CreateAsync(CreateProVietKenoRound kenoRound)
        {
            var sum = kenoRound.Result.Sum(x => x);

            var entity = new ProVietKenoRoundEntity
            {
                Time = kenoRound.Time,
                Result = kenoRound.Result,
                Number = kenoRound.Number,
                SettlementResult = new SettlementResult()
            };

            SetBigSmallValue(entity, sum);
            SetOddEvenValue(entity, sum);
            SetElementValue(entity, sum);

            await repository.InsertOneAsync(entity);
        }

        private void SetElementValue(ProVietKenoRoundEntity entity, int sum)
        {
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
        }

        private void SetOddEvenValue(ProVietKenoRoundEntity entity, int sum)
        {
            if (sum % 2 == 0)
            {
                entity.SettlementResult.Even = true;
            }
            else
            {
                entity.SettlementResult.Odd = true;
            }
        }

        private void SetBigSmallValue(ProVietKenoRoundEntity entity, int sum)
        {
            if (sum >= 811)
            {
                entity.SettlementResult.Big = true;
            }
            else
            {
                entity.SettlementResult.Small = true;
            }
        }
    }
}