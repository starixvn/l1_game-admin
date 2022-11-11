using System.Collections.Generic;

namespace GamesAdmin.Site.Features.CurrencyGroups.Models
{
    public class CreateCurrencyGroupModel
    {
        public string Name { get; set; }

        public string PartnerId { get; set; }

        public bool Enabled { get; set; }

        public bool FractionalOddsFormat { get; set; }

        public IEnumerable<string> Currencies { get; set; }
    }
}