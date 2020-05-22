using System;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Workers.Repository;
using GamesAdmin.Workers.Repository.Entities;

namespace GamesAdmin.Workers.Services
{
    public interface IKenoService
    {
        Task AddNewRoundResultAsync(VietLottKenoRound kenoRound);
    }

    public class KenoService : IKenoService
    {
        private readonly IKenoRepository kenoRepository;

        public KenoService(IKenoRepository kenoRepository)
        {
            this.kenoRepository = kenoRepository;
        }

        public async Task AddNewRoundResultAsync(VietLottKenoRound kenoRound)
        {
            var entity = new KenoRoundEntity
            {
                Time = DateTime.UtcNow,
                RoundNo = kenoRound.DrawId,
                Results = kenoRound.Data.Split("|").Select(x => byte.Parse(x)).OrderBy(x => x)
            };

            await kenoRepository.InsertAsync(entity);
        }
    }
}