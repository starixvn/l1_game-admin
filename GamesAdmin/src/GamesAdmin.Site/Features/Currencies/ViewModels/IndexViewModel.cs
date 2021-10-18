using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class IndexViewModel
    {
        public string PartnerId { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }
    }
}