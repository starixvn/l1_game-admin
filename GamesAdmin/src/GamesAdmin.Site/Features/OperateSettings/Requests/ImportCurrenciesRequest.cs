using System.Collections.Generic;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Site.Features.OperateSettings.Requests
{
    public class ImportCurrenciesRequest
    {
       public IEnumerable<OperatorCurrency> OperatorCurrencies { get; set; }
    }

    public class OperatorCurrency
    {
        public string Operator { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }
    }
}
