using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Partners;
using Newtonsoft.Json;

namespace GamesAdmin.Site.Features.OperateSettings.ViewModels
{
    public class CurrencySettingViewModel
    {
        public IEnumerable<CurrencyByOperatorViewModel> OperatorCurrencies { get; set; }
    }

    public class CurrencyByOperatorViewModel 
    {
        public Partner Operator { get; set; }

        public IEnumerable<CurrencyModel> Currencies { get; set; }


        public string BuildCurrencyJson()
        {
            return JsonConvert.SerializeObject(Currencies.Select(currency => new
            {
                currency.Name,
                currency.Code,
                currency.Multiplier,
                currency.IsTest,
                currency.IsUncountOnTotal,
                currency.DisplayText
            }), new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            });
        }
    }
}
