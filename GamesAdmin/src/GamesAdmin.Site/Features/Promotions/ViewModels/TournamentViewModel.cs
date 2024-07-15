using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class TournamentViewModel
    {
        public IEnumerable<SelectListItem> StatusItem => new List<SelectListItem>{
            new SelectListItem(PromotionCampaignStatus.All.DisplayName, PromotionCampaignStatus.All.Value),
            new SelectListItem(PromotionCampaignStatus.Running.DisplayName, PromotionCampaignStatus.Running.Value),
            new SelectListItem(PromotionCampaignStatus.Completed.DisplayName, PromotionCampaignStatus.Completed.Value)
        };

        public string Status { get; set; }
    }
}
