using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CurrencyGroupViewModel
    {
        public IEnumerable<SelectListItem> CurencyItems { get; set; }

        public IEnumerable<SelectListItem> PartnerItems { get; set; }
    }
}