using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CurrencyManagementViewModel
    {
        public IEnumerable<CurrencyModel> Models { get; set; }
    }
}