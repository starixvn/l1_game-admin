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
                Results = x.Results,
                Number = x.Number
            });
        }
    }
}