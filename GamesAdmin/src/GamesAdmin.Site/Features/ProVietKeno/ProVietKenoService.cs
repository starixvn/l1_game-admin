using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Sentry;

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
        private readonly ISentryClient sentryClient;

        public ProVietKenoService(IProVietKenoApi provietKenoApi, ISentryClient sentryClient)
        {
            this.provietKenoApi = provietKenoApi;
            this.sentryClient = sentryClient;
        }

        public Task<IEnumerable<ProVietKenoRound>> GetRoundsAsync(DateTime date)
        => provietKenoApi.GetRounds(date.ToString("o"));

        public Task<ProVietKenoRound> CreateAsync(CreateProVietKenoRound kenoRound)
        => provietKenoApi.Create(kenoRound);

        public Task<ProVietKenoRound> GetByRoundIdAsync(long roundId)
        => provietKenoApi.GetByRoundId(roundId);

        public async Task<IEnumerable<ProVietKenoRound>> GetUnsettledRoundsAsync()
        {
            try
            {
                return await provietKenoApi.GetUnsettledRounds();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return default;
        }
    }
}