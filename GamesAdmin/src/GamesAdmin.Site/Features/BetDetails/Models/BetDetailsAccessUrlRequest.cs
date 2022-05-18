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
                OperatorId = model.OperatorId,
                OperatorSecret = model.OperatorSecret
            };
            Language = model.Language;
            GameRoundId = model.RoundId;
            GameTypeId = model.GameType;
            PlayerId = model.PlayerId;
        }
        public long GameRoundId { get; set; }

        public int PlayerId { get; set; }

        public byte GameTypeId { get; set; }

        public Auth Auth { get; set; }

        public string Language { get; set; }
    }
}
