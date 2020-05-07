using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
                Date = DateTime.Now,
                RoundId = kenoRound.DrawId,
                Results = kenoRound.Data.Split("|").Select(x => int.Parse(x))
            };

            await kenoRepository.InsertAsync(entity);
        }
    }
}