using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<Notification> notifications)
        {
            Notifications = notifications;
        }

        public IList<Notification> Notifications { get; }
    }
}