using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.ProVietKeno
{
    public interface IProVietKenoApi : IBaseAuthorizationApi
    {
        [Get("/proviet_keno/rounds/{date}")]
        Task<IEnumerable<ProVietKenoRound>> GetRounds(string date);

        [Post("/proviet_keno/rounds/create")]
        Task<ProVietKenoRound> Create([Body] CreateProVietKenoRound kenoRound);

        [Get("/proviet_keno/rounds/id/{roundId}")]
        Task<ProVietKenoRound> GetByRoundId(long roundId);

        [Get("/proviet_keno/rounds/unsettled")]
        Task<IEnumerable<ProVietKenoRound>> GetUnsettledRounds();

        [Post("/game_server/notify_proviet_results")]
        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);
    }
}