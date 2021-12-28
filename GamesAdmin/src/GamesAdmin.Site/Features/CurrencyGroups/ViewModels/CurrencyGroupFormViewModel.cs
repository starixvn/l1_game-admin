using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CurrencyGroupFormViewModel
    {
        public IEnumerable<SelectListItem> CurrencyItems { get; set; }

        public IEnumerable<SelectListItem> PartnerItems { get; set; }
    }
}