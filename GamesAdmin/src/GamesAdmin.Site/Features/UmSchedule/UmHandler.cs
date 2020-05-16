using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.UmSchedule
{
    public class UmHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<AddRequest, AddViewModel>
    {
        public Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new IndexViewModel());

        public Task<AddViewModel> Handle(AddRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new AddViewModel());
    }
}
