

namespace GamesAdmin.Site.Features.BetDetails.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using GamesAdmin.Core.Models.Partners;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class BetDetailsViewModel
    {
        public IEnumerable<Partner> Operators { get; set; } = new List<Partner>();

        public IEnumerable<SelectListItem> OperatorItem => Operators.Select(o => new SelectListItem(o.PartnerId, o.PartnerId));

        public IEnumerable<SelectListItem> LanguageItem { get; set; }

        public int RoundId { get; set; }

        public int PlayerId { get; set; }

        public string Language { get; set; }

        public int GameType { get; set; }

        public string OperatorId { get; set; }

        public string OperatorSecret { get; set; }

        public string SiteId { get; set; } = "4100300";

        public bool FreeBet { get; set; }
    }
}
