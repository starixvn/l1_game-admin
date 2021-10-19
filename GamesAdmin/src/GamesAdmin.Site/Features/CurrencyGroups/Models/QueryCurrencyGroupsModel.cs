using System.Collections.Generic;

namespace GamesAdmin.Site.Features.CurrencyGroups.Models
{
    public class QueryCurrencyGroupsModel
    {
        public IEnumerable<string> Currencies { get; set; }

        public IEnumerable<string> Partners { get; set; }
    }
}