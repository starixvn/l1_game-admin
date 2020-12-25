using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public class ResultMonitorHandler : IRequestHandler<SetSlotResultRequest, SettlementBoardViewModel>
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
    }
}