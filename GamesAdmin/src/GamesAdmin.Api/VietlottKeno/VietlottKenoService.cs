using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.VietlottKeno
{
    public interface IVietlottKenoService
    {
        Task<IEnumerable<VietlottKenoRound>> GetRoundsAsync(DateTime date);

        Task CreateAsync(CreateVietlottKenoRound kenoRound);
    }

    public class VietlottKenoService : IVietlottKenoService
    {
        private IGenericRepository<VietlottKenoRoundEntity> repository;

        public VietlottKenoService(IGenericRepository<VietlottKenoRoundEntity> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<VietlottKenoRound>> GetRoundsAsync(DateTime date)
        {
            var fromtDate = date;
            var toDate = fromtDate.AddDays(1);

            var result = await repository.FilterByAsync(x => x.Time >= fromtDate && x.Time < toDate);

            return result.Select(x => new VietlottKenoRound
            {
                Time = x.Time,
                Result = x.Result,
                Number = x.Number,
                Big = x.SettlementResult.Big,
                Even = x.SettlementResult.Even,
                Earth = x.SettlementResult.Earth,
                Fire = x.SettlementResult.Fire,
                Gold = x.SettlementResult.Gold,
                Wood = x.SettlementResult.Wood,
                Water = x.SettlementResult.Water
            });
        }

        public async Task CreateAsync(CreateVietlottKenoRound kenoRound)
        {
            var sum = kenoRound.Result.Sum(x => x);

            var entity = new VietlottKenoRoundEntity
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

        private void SetElementValue(VietlottKenoRoundEntity entity, int sum)
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

        private void SetOddEvenValue(VietlottKenoRoundEntity entity, int sum)
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

        private void SetBigSmallValue(VietlottKenoRoundEntity entity, int sum)
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