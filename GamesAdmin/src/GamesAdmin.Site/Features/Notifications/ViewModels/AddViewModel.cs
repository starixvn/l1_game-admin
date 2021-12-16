using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class AddViewModel
    {
        private const int MinLength = 20;
        private const int MaxLength = 1000;

        public AddViewModel()
        {
            DisplayKinds = EnumerationHelper.ToSelectList<NotificationDisplayKind>();
        }

        public List<SelectListItem> DisplayKinds { get; }

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
    }
}
