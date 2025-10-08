using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Currencies.ViewModels
{
    public class CurrencyManagementViewModel
    {
        public IEnumerable<CurrencyModel> Models { get; set; }

        public IEnumerable<Partner> Partners { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroups { get; set; }

        public Partner GetPartner(CurrencyModel model)
        => Partners.FirstOrDefault(partner => partner.PartnerId.Equals(model.PartnerId));

        public CurrencyGroupModel GetCurrencyGroup(CurrencyModel model)
        => CurrencyGroups.FirstOrDefault(group => group.Id.Equals(model.GroupId));
    }
}