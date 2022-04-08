using System.Collections.Generic;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public interface IResultMonitorApi : IBaseAuthorizationApi
    {
        [Post("/game_server/slot/set_board")]
        Task<SettlementBoard> SetSlotResult([FromBody] SetSlotResultRequest request);

        [Post("/game_server/casinoholdempoker/set_result")]
        Task<CasinoHoldemPokerResultControl> SetCasinoHoldemPokerResult([FromBody] SetCasinoHoldemPokerResultRequest request);
        [Post("/game_server/hilo/set_result")]
        Task<IEnumerable<Card>> SetHiLoResult([FromBody] SetHiLoResultRequest request);
    }

    public interface IResultMonitorService
    {
        Task<SettlementBoard> SetSlotResult(SetSlotResultRequest request);

        Task<CasinoHoldemPokerResultControl> SetCasinoHoldemPokerResult(SetCasinoHoldemPokerResultRequest request);
        Task<IEnumerable<Card>> SetHiLoResult(SetHiLoResultRequest request);
    }

    public class ResultMonitorService : IResultMonitorService
    {
        private readonly IResultMonitorApi api;

        public ResultMonitorService(IResultMonitorApi api)
        {
            this.api = api;
        }

        public Task<SettlementBoard> SetSlotResult(SetSlotResultRequest request)
        => api.SetSlotResult(request);

        public Task<CasinoHoldemPokerResultControl> SetCasinoHoldemPokerResult(SetCasinoHoldemPokerResultRequest request)
      => api.SetCasinoHoldemPokerResult(request);
        public Task<IEnumerable<Card>> SetHiLoResult(SetHiLoResultRequest request)
            => api.SetHiLoResult(request);
    }
}