using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications
{
    public class NotificationHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<AddRequest, bool>
    {
        private readonly IMapper mapper;
        private readonly INotificationService notificationService;

        public NotificationHandler(IMapper mapper, INotificationService notificationService)
        {
            this.mapper = mapper;
            this.notificationService = notificationService;
        }

        public async Task<IndexViewModel> Handle(IndexRequest request, CancellationToken cancellationToken)
        {
            var notifications = await notificationService.Get();

            return new IndexViewModel(notifications);
        }

        public Task<bool> Handle(AddRequest request, CancellationToken cancellationToken)
        {
            var notification = mapper.Map<Notification>(request);

            return notificationService.Add(notification);
        }
    }
}
