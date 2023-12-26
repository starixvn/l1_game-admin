namespace GamesAdmin.Site.Features.BetDetails.Models
{
    using GamesAdmin.Site.Features._Shared;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;

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
