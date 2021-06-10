using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Notifications.Requests;
using GamesAdmin.Site.Features.Notifications.ViewModels;
using GamesAdmin.Site.Helpers;
using MediatR;

namespace GamesAdmin.Site.Features.Notifications
{
    public class NotificationHandler
        : IRequestHandler<IndexRequest, IndexViewModel>
        , IRequestHandler<GetByIdRequest, EditViewModel>
        , IRequestHandler<AddRequest, bool>
        , IRequestHandler<UpdateRequest, bool>
        , IRequestHandler<DeleteRequest, bool>
    {
        private const string NewLinePattern = @"\t|\n|\r";
        private const string Space = " ";

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
            request.EnMessage = Regex.Replace(request.EnMessage, NewLinePattern, Space);
            request.VnMessage = Regex.Replace(request.VnMessage, NewLinePattern, Space);
            var notification = new Notification
            {
                DisplayKind = request.DisplayKind,
                Messages = new Dictionary<string, string> 
                {
                    {Language.VnValue, request.VnMessage },
                    {Language.EnValue, request.EnMessage }
                },
                Message = request.VnMessage
            };

            return notificationService.Add(notification);
        }

        public async Task<EditViewModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var notification = await notificationService.GetById(request.Id);

            return new EditViewModel(EnumerationHelper.ToSelectList<NotificationDisplayKind>(), notification);
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            request.EnMessage = Regex.Replace(request.EnMessage, NewLinePattern, Space);
            request.VnMessage = Regex.Replace(request.VnMessage, NewLinePattern, Space);

            var notification = new Notification
            {
                Id = request.Id,
                DisplayKind = request.DisplayKind,
                Messages = new Dictionary<string, string>
                {
                    {Language.VnValue, request.VnMessage },
                    {Language.EnValue, request.EnMessage }
                },
                Message = request.VnMessage
            };

            return notificationService.Update(notification);
        }

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => notificationService.Delete(request.Id);
    }
}
