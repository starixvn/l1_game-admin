using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Currencies;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CurrencyManagementViewModel
    {
        public IEnumerable<CurrencyModel> Models { get; set; }

        public IEnumerable<SelectListItem> Partners { get; set; }

        public string GetPartnerName(CurrencyModel model)
        => Partners.FirstOrDefault(partner => partner.Value.Equals(model.PartnerId))?.Text;
    }
}