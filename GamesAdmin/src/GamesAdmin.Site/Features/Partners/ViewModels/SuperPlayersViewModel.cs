using GamesAdmin.Core.Models.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Partners.ViewModels
{
    public class SuperPlayersViewModel
    {
        public SuperPlayersViewModel(IEnumerable<Partner> partners)
        {
            PartnerSelectList = partners.Select(partner => new SelectListItem(partner.Name, partner.PartnerId)).ToList();
        }

        public string PartnerId { get; set; }

        public string SuperPlayers { get; set; }

        public List<SelectListItem> PartnerSelectList { get; set; }
    }
}
