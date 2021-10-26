using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Partners;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class IndexViewModel : CurrencyGroupFormViewModel
    {
        public IEnumerable<CurrencyModel> CurrencyModels { get; set; }

        public IEnumerable<Partner> Partners { get; set; }

        public IEnumerable<string> Currencies { get; set; }
    }
}