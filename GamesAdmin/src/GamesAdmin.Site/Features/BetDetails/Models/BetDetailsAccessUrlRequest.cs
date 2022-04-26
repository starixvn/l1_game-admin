namespace GamesAdmin.Site.Features.BetDetails.Models
{
    using GamesAdmin.Core.Models.ApiLogs;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;

    public class BetDetailsAccessUrlRequest
    {
        public BetDetailsAccessUrlRequest(BetDetailsViewModel model)
        {
            Auth = new Auth
            {
                PartnerId = model.OperatorId,
                PartnerSecret = model.OperatorSecret
            };
            Language = model.Language;
            GameRoundId = model.RoundId;
            GameTypeId = model.GameType;
            ObCustId = model.PlayerId;
        }
        public long GameRoundId { get; set; }

        public int ObCustId { get; set; }

        public byte GameTypeId { get; set; }

        public Auth Auth { get; set; }

        public string Language { get; set; }
    }
}
