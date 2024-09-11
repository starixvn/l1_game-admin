using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameGroupManagement.ViewModels
{
    public class CreateGameGroupViewModel
    {
        [DisplayName("Group name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Games")]
        [Required]
        public IEnumerable<int> GameIds { get; set; } = Enumerable.Empty<int>();

        public IEnumerable<SelectListItem> Games { get; set; }
    }
}