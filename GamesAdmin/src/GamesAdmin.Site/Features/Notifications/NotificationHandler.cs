using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications
{
    public class NotificationHandler
        : IRequestHandler<IndexRequest, IndexViewModel>        
        , IRequestHandler<GetByIdRequest, EditViewModel>
        , IRequestHandler<AddRequest, bool>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
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

        public async Task<EditViewModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var notification = await notificationService.GetById(request.Id);

            return new EditViewModel(GenerateEventTypes(), GenerateDisplayKinds(), notification);
        }

        private static List<SelectListItem> GenerateEventTypes()
        => Enumeration.GetAll<EventScheduleType>().Select(x => new SelectListItem(x.DisplayName, x.Value)).ToList();

        private static List<SelectListItem> GenerateDisplayKinds()
        => Enumeration.GetAll<NotificationDisplayKind>().Select(x => new SelectListItem(x.DisplayName, x.Value)).ToList();

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var notification = mapper.Map<Notification>(request);

            return notificationService.Update(notification);
        }

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => notificationService.Delete(request.Id);
    }
}
