using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.ProVietKeno
{
    public interface IProVietKenoService
    {
        Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date);

        Task<ProVietKenoRound> CreateAsync(CreateProVietKenoRound kenoRound);

        Task<ProVietKenoRound> GetByRoundIdAsync(long roundId);

        Task<IEnumerable<ProVietKenoRound>> GetUnsettledRoundsAsync();
    }

    public class ProVietKenoService : IProVietKenoService
    {
        private readonly IProVietKenoApi provietKenoApi;

        public ProVietKenoService(IProVietKenoApi provietKenoApi)
        {
            this.provietKenoApi = provietKenoApi;
        }

        public async Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date)
        => await provietKenoApi.GetRounds(date);

        public async Task<ProVietKenoRound> CreateAsync(CreateProVietKenoRound kenoRound)
        => await provietKenoApi.Create(kenoRound);

        public async Task<ProVietKenoRound> GetByRoundIdAsync(long roundId)
        => await provietKenoApi.GetByRoundId(roundId);

        public async Task<IEnumerable<ProVietKenoRound>> GetUnsettledRoundsAsync()
        {
            try
            {
                return await provietKenoApi.GetUnsettledRounds();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}