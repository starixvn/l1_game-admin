using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Um;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.UmSchedule
{
    public class UmHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<CreateRequest, bool>
        , IRequestHandler<EditRequest, EditViewModel>
    {
        private readonly IUmService umService;
        private readonly IMapper mapper;

        public UmHandler(IUmService umService, IMapper mapper)
        {
            this.umService = umService;
            this.mapper = mapper;
        }

        public Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new IndexViewModel());

        public Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new EditViewModel());

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => umService.Add(mapper.Map<EventSchedule>(request));
    }
}
