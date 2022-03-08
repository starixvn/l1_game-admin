using AutoMapper;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Site.Features.Report.ViewModels;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    public class ResultMonitorHandler :
        IRequestHandler<SetSlotResultRequest, SettlementBoardViewModel>,
        IRequestHandler<SetHoldemPokerResultRequest, HoldemPokerResultControl>,
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
            => new SettlementBoardViewModel(request.GameConfigName, await service.SetSlotResult(request));

        public async Task<HoldemPokerResultControl> Handle(SetHoldemPokerResultRequest request, CancellationToken cancellationToken)
            => await service.SetHoldemPokerResult(request);

        public async Task<IEnumerable<CardViewModel>> Handle(SetHiLoResultRequest request, CancellationToken cancellationToken)
        {
            var result = await service.SetHiLoResult(request);

            return result.Select(x => mapper.Map<CardViewModel>(x));
        }
    }
}