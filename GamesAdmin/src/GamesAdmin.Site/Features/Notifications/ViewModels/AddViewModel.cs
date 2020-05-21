using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class AddViewModel
    {
        public AddViewModel(
            List<SelectListItem> eventTypes,
            List<SelectListItem> displayKinds)
        {
            EventTypes = eventTypes;
            DisplayKinds = displayKinds;
        }

        public List<SelectListItem> EventTypes { get; }

        public List<SelectListItem> DisplayKinds { get; }

        public EventScheduleType EventType { get; set; }

        public NotificationDisplayKind DisplayKind { get; set; }

        public string Message { get; set; }
    }
}
