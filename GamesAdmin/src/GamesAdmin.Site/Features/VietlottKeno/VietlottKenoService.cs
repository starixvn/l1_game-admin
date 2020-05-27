using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.VietlottKeno
{
    public interface IVietlottKenoService
    {
        Task<IEnumerable<VietlottKenoRound>> GetRoundsAsync(DateTime date);

        Task CreateAsync(CreateVietlottKenoRound kenoRound);
    }

    public class VietlottKenoService : IVietlottKenoService
    {
        private IVietlottKenoApi vietlottKenoApi;

        public VietlottKenoService(IVietlottKenoApi vietlottKenoApi)
        {
            this.vietlottKenoApi = vietlottKenoApi;
        }

        public async Task<IEnumerable<VietlottKenoRound>> GetRoundsAsync(DateTime date)
        => await vietlottKenoApi.GetRounds(date);

        public async Task CreateAsync(CreateVietlottKenoRound kenoRound)
        => await vietlottKenoApi.Create(kenoRound);
    }
}