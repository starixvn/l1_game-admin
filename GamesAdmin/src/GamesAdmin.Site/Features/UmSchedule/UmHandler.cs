using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.UmSchedule
{
    public class UmHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<AddRequest, AddViewModel>
        , IRequestHandler<EditRequest, EditViewModel>
    {
        private readonly IAppSettings appSettings;

        public UmHandler(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new IndexViewModel());

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel(appSettings));

        public Task<EditViewModel> Handle(EditRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new EditViewModel());
    }
}
