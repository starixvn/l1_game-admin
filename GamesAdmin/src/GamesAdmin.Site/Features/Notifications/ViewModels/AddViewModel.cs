using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Notifications.ViewModels
{
    public class AddViewModel
    {
        public AddViewModel()
        {
            DisplayKinds = EnumerationHelper.ToSelectList<NotificationDisplayKind>();
        }

        public List<SelectListItem> DisplayKinds { get; }

        public byte EventTypeValue { get; set; }

        public byte DisplayKindValue { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
