using GamesAdmin.Core.Models.RoundResults;
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
    }

    public interface IResultMonitorService
    {
        Task<SettlementBoard> SetSlotResult(SetSlotResultRequest request);

        Task<CasinoHoldemPokerResultControl> SetCasinoHoldemPokerResult(SetCasinoHoldemPokerResultRequest request);
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
    }
}