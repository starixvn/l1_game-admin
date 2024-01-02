using GamesAdmin.Site.Features.BetDetails.ViewModels;

namespace GamesAdmin.Site.Features.BetDetails.Models
{
    public class BetDetailsAccessUrlApiRequest : BetHistoryApiRequest
    {
        public BetDetailsAccessUrlApiRequest(BetDetailsViewModel model)
            : base(model)
        {
            Language = model.Language;
        }

        public string Language { get; set; }
    }
}