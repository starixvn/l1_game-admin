using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<string> PartnerIds { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }
    }
}