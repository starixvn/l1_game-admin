using System.Threading.Tasks;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public interface IResultMonitorApi : IBaseAuthorizationApi
    {
        [Post("/game_server/slot/set_board")]
        Task<SettlementBoard> SetSlotResult([FromBody] SetSlotResultRequest request);
    }

    public interface IResultMonitorService
    {
        Task<SettlementBoard> SetSlotResult(SetSlotResultRequest request);
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
    }
}