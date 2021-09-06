using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.UmSchedule
{
    public class UmHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
    {
        private readonly IUmService umService;
        private readonly IMapper mapper;

        public UmHandler(IUmService umService, IMapper mapper)
        {
            this.umService = umService;
            this.mapper = mapper;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            var schedules = (await umService.GetUnfinished());

            return new IndexViewModel(mapper.Map<List<UmScheduleItemViewModel>>(schedules));
        }
    }
}