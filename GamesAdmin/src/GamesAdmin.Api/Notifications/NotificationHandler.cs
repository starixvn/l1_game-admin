using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Notifications
{
    public class NotificationHandler 
        : IRequestHandler<AddRequest, bool>
        , IRequestHandler<GetRequest, IList<Notification>>
        , IRequestHandler<GetByIdRequest, Notification>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
    {
        private readonly INotificationService notificationService;

        public NotificationHandler(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public Task<bool> Handle(AddRequest request, CancellationToken cancellationToken)
        => notificationService.Create(request);

        public Task<IList<Notification>> Handle(GetRequest request, CancellationToken cancellationToken)
        => notificationService.Get();

        public Task<Notification> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        => notificationService.GetById(request.Id);

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        => notificationService.Update(request);

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => notificationService.Delete(request.Id);
    }
}
