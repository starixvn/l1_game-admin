    using GamesAdmin.Core.Extensions;
using GamesAdmin.Site.Features.BetDetails.ViewModels;

namespace GamesAdmin.Site.Features.BetDetails.Models
{
    public class BetDetailsAccessUrlApiRequest : BetHistoryApiRequest
    {
        public BetDetailsAccessUrlApiRequest(BetDetailsViewModel model)
            : base(model)
        {
            Language = model.Language;
            PlayerId = model.PlayerId;
        }
        public string PlayerId { get; set; }

        public string Language { get; set; }
    }
}