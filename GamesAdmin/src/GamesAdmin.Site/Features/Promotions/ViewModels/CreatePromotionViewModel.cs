using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.Promotions.ViewModels
{
    public class CreatePromotionViewModel : PromotionViewModel
    {
        public IEnumerable<SelectListItem> GameList { get; set; }
    }
}