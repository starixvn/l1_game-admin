using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Partners;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Lobby.ViewModels
{
    public class CurrencyGroupViewModel
    {
        public IEnumerable<Partner> Partners { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroupModels { get; set; }
    }
}
