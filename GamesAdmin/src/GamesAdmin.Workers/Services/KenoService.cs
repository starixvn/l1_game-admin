using System;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Workers.Services
{
    public interface IKenoService
    {
        Task AddNewRoundResultAsync(VietLottKenoRound kenoRound);
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

            await repository.InsertOneAsync(entity);
        }
    }
}