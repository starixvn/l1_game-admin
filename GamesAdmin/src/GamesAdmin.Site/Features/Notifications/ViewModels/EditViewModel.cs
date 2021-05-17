using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class EditViewModel
    {
        private const int MinLength = 20;
        private const int MaxLength = 1000;

        public EditViewModel(
            List<SelectListItem> displayKinds,
            Notification notification)
        {
            DisplayKinds = displayKinds;
            Id = notification.Id;
            VnMessage = notification.Messages?[Language.VnValue] != null ? notification.Messages[Language.VnValue] : notification.Message;
            DisplayKindValue = byte.Parse(notification.DisplayKind.Value);
        }

        public List<SelectListItem> DisplayKinds { get; }

        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string VnMessage { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string EnMessage { get; set; }

        [Required]
        public byte DisplayKindValue { get; set; }
    }
}