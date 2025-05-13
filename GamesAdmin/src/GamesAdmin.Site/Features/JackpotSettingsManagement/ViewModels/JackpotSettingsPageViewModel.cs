using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels
{
    public class JackpotSettingsPageViewModel
    {
        [DisplayName("Games")]
        [Required]
        public IEnumerable<int> GameIds { get; set; }

        public IEnumerable<SelectListItem> Games { get; set; }

        public string SiteNames { get; set; }
    }
}
