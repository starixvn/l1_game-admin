using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Currencies.Models
{
    public class GetCurrencyByPartnersModel
    {
        public IEnumerable<string> PartnerIds { get; set; }
    }
}