using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Notifications
{
    public class NotificationHandler 
        : IRequestHandler<CreateRequest, bool>
        , IRequestHandler<GetRequest, IList<Notification>>
    {
        private readonly NotificationService notificationService;

        public NotificationHandler(NotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        => notificationService.Create(new Notification(string.Empty, request.Message, request.EventType, request.DisplayKind));

        public Task<IList<Notification>> Handle(GetRequest request, CancellationToken cancellationToken)
        => notificationService.Get();
    }
}
