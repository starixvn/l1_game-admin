using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class EditViewModel
    {
        public EditViewModel(
            List<SelectListItem> eventTypes,
            List<SelectListItem> displayKinds,
            Notification notification)
        {
            EventTypes = eventTypes;
            DisplayKinds = displayKinds;
            Id = notification.Id;
            Message = notification.Message;
            EventTypeValue = byte.Parse(notification.EventType.Value);
            DisplayKindValue = byte.Parse(notification.DisplayKind.Value);
        }

        public List<SelectListItem> EventTypes { get; }

        public List<SelectListItem> DisplayKinds { get; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public byte EventTypeValue { get; set; }

        [Required]
        public byte DisplayKindValue { get; set; }
    }
}
