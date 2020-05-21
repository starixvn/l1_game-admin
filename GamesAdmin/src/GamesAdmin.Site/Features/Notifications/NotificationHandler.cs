using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications
{
    public class NotificationHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
    {
        public Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new IndexViewModel());
        }
    }
}
