using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> StatusItem => new List<SelectListItem>{
            new SelectListItem(PromotionStatus.All.DisplayName, PromotionStatus.All.Value),
            new SelectListItem(PromotionStatus.Enabled.DisplayName, PromotionStatus.Enabled.Value),
            new SelectListItem(PromotionStatus.Disabled.DisplayName, PromotionStatus.Disabled.Value)
        };

        public string Status { get; set; }
    }
}
