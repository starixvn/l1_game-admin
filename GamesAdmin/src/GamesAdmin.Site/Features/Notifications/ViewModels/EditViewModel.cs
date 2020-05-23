using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class EditViewModel
    {
        public EditViewModel(
            List<SelectListItem> displayKinds,
            Notification notification)
        {
            DisplayKinds = displayKinds;
            Id = notification.Id;
            Message = notification.Message;
            DisplayKindValue = byte.Parse(notification.DisplayKind.Value);
        }

        public List<SelectListItem> DisplayKinds { get; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public byte DisplayKindValue { get; set; }
    }
}
