using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Currencies;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CurrencyGroupManagementViewModel : CurrencyGroupViewModel
    {
        public IEnumerable<CurrencyGroupModel> Models { get; set; }

        public string GetPartnerName(CurrencyGroupModel model)
            => PartnerItems.FirstOrDefault(partner => partner.Value.Equals(model.PartnerId))?.Text;

        public string GetCurrenciesSeperatedByComma(CurrencyGroupModel model)
            => string.Join(",", model.Currencies.Select(currency => currency.Name));
    }
}