using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class IndexViewModel : CurrencyGroupFormViewModel
    {
        public IEnumerable<Partner> Partners { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroupModels { get; set; }

        public IEnumerable<string> CurrencyGroups { get; set; }

        public IEnumerable<SelectListItem> CurrencyGroupItems { get; set; }
    }
}