using System.Collections.Generic;
using GamesAdmin.Site.Features.GameSettings.ViewModels;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class UpdatePromotionViewModel : PromotionViewModel
    {
        public IList<OddsViewModel> OriginalOdds { get; set; }
    }
}
