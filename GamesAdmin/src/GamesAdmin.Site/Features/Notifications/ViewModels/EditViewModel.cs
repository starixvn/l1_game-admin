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

        public EditViewModel(List<SelectListItem> displayKinds, Notification notification)
        {
            DisplayKinds = displayKinds;
            Id = notification.Id;
            DisplayKindValue = byte.Parse(notification.DisplayKind.Value);

            VNMessage = GetMessage(notification, Language.VN);
            ENMessage = GetMessage(notification, Language.EN);
            CNMessage = GetMessage(notification, Language.CN);
            TWMessage = GetMessage(notification, Language.TW);
            THMessage = GetMessage(notification, Language.TH);
            IDMessage = GetMessage(notification, Language.ID);
        }

        public List<SelectListItem> DisplayKinds { get; }

        [Required]
        public string Id { get; set; }

        [Required]
        public byte DisplayKindValue { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string VNMessage { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string ENMessage { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string CNMessage { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string TWMessage { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string THMessage { get; set; }

        [Required]
        [MinLength(MinLength)]
        [MaxLength(MaxLength)]
        public string IDMessage { get; set; }

        private string GetMessage(Notification notification, string language)
        {
            return notification.Messages == null || !notification.Messages.ContainsKey(language)
                ? notification.Message : notification.Messages[language];
        }
    }
}