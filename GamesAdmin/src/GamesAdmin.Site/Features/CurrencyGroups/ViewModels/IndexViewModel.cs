using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class IndexViewModel : CurrencyGroupViewModel
    {
        public IEnumerable<string> Currencies { get; set; }

        public IEnumerable<string> Partners { get; set; }
    }
}