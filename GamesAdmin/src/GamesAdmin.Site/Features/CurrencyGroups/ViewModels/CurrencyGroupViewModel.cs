using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CurrencyGroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public string PartnerId { get; set; }

        public bool FractionalOddsFormat { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }

        public IEnumerable<string> Games { get; set; }

        public string GetCurrenciesSeperatedByComma()
            => string.Join(", ", Currencies.Select(currency => currency.Name));

        public string GetGamesSeperatedByComma()
            => string.Join(", ", Games.Select(GetGameName));

        private static string GetGameName(string key)
            => Enumeration.Contains<Game>(key) ? Enumeration.FromValue<Game>(key).DisplayName : key;
    }
}