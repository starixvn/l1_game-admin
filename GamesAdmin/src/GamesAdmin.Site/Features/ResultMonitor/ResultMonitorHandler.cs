using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public class ResultMonitorHandler :
        IRequestHandler<SetSlotResultRequest, SettlementBoardViewModel>,
        IRequestHandler<SetCasinoHoldemPokerResultRequest, CasinoHoldemPokerResultControl>
    {
        private readonly IResultMonitorService service;

        public ResultMonitorHandler(IResultMonitorService service)
        {
            this.service = service;
        }

        public async Task<SettlementBoardViewModel> Handle(SetSlotResultRequest request, CancellationToken cancellationToken)
        {
            var settlementBoard = await service.SetSlotResult(request);

            return new SettlementBoardViewModel(request.GameConfigName, settlementBoard);
        }

        public async Task<CasinoHoldemPokerResultControl> Handle(SetCasinoHoldemPokerResultRequest request, CancellationToken cancellationToken)
        {
            return await service.SetCasinoHoldemPokerResult(request);
        }
    }
}