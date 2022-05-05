using System.Collections.Generic;
using System.Linq;
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
            request.VNMessage = Regex.Replace(request.VNMessage, NewLinePattern, Space);
            request.ENMessage = Regex.Replace(request.ENMessage, NewLinePattern, Space);
            request.CNMessage = Regex.Replace(request.CNMessage, NewLinePattern, Space);
            request.TWMessage = Regex.Replace(request.TWMessage, NewLinePattern, Space);
            request.THMessage = Regex.Replace(request.THMessage, NewLinePattern, Space);
            request.IDMessage = Regex.Replace(request.IDMessage, NewLinePattern, Space);

            var messages = new Dictionary<string, string>();
            var type = request.GetType();
            foreach (var l in Language.SupportedLanguages)
            {
                messages[l.Key] = (string)type.GetProperty(Language.LanguageMapper[l.Key]).GetValue(request);
            }

            var notification = new Notification
            {
                DisplayKind = request.DisplayKind,
                Messages = messages,
                Message = request.VNMessage
            };

            return notificationService.Add(notification);
        }

        public async Task<EditViewModel> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var notification = await notificationService.GetById(request.Id);
            var displayKinds = EnumerationHelper.ToSelectList<NotificationDisplayKind>()
                .Where(d => d.Value != NotificationDisplayKind.DisabledValue)
                .ToList();

            return new EditViewModel(displayKinds, notification);
        }

        public Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            request.VNMessage = Regex.Replace(request.VNMessage, NewLinePattern, Space);
            request.ENMessage = Regex.Replace(request.ENMessage, NewLinePattern, Space);
            request.CNMessage = Regex.Replace(request.CNMessage, NewLinePattern, Space);
            request.TWMessage = Regex.Replace(request.TWMessage, NewLinePattern, Space);
            request.THMessage = Regex.Replace(request.THMessage, NewLinePattern, Space);
            request.IDMessage = Regex.Replace(request.IDMessage, NewLinePattern, Space);

            var messages = new Dictionary<string, string>();
            var type = request.GetType();
            foreach (var l in Language.SupportedLanguages)
            {
                messages[l.Key] = (string)type.GetProperty(Language.LanguageMapper[l.Key]).GetValue(request);
            }

            var notification = new Notification
            {
                Id = request.Id,
                DisplayKind = request.DisplayKind,
                Messages = messages,
                Message = request.VNMessage
            };

            return notificationService.Update(notification);
        }

        public Task<bool> Handle(DeleteRequest request, CancellationToken cancellationToken)
        => notificationService.Delete(request.Id);
    }
}
