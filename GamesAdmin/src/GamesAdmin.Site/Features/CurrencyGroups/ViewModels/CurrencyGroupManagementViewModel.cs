using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.CurrencyGroups.ViewModels
{
    public class CurrencyGroupManagementViewModel : CurrencyGroupFormViewModel
    {
        public IEnumerable<CurrencyGroupViewModel> Models { get; set; }

        public string GetPartnerName(CurrencyGroupViewModel model)
            => PartnerItems.FirstOrDefault(partner => partner.Value.Equals(model.PartnerId))?.Text;
    }
}