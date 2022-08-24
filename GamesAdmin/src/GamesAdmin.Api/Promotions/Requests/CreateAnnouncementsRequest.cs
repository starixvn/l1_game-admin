using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Api.Notifications.Models;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class CreateAnnouncementsRequest : IRequest<bool>
    {
        public CreateAnnouncementsRequest(IEnumerable<Notification> notifications)
        {
            Notifications = notifications;
        }

        public IEnumerable<Notification> Notifications { get; }

        public IEnumerable<CreateAnnouncementModel> Models => Notifications.Select(notification => new CreateAnnouncementModel(notification));
    }
}
