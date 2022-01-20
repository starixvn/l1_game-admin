using System.Collections.Generic;
using GamesAdmin.Core.Models.RoundResults;
using System.Linq;
using AutoMapper;
using GamesAdmin.Site.Features.Report.ViewModels;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public class ResultMonitorHandler : 
        IRequestHandler<SetSlotResultRequest, SettlementBoardViewModel>,
        IRequestHandler<SetCasinoHoldemPokerResultRequest, CasinoHoldemPokerResultControl>,
        IRequestHandler<SetHiLoResultRequest, IEnumerable<CardViewModel>>
    {
        private readonly IResultMonitorService service;
        private readonly IMapper mapper;

        public ResultMonitorHandler(IResultMonitorService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
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
        
        public async Task<IEnumerable<CardViewModel>> Handle(SetHiLoResultRequest request, CancellationToken cancellationToken)
        {
            var result = await service.SetHiLoResult(request);

            return result.Select(x => mapper.Map<CardViewModel>(result));
        }
    }
}