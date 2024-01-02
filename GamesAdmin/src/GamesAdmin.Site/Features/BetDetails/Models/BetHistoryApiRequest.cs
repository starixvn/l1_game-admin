using GamesAdmin.Core.Models.ApiLogs;
using GamesAdmin.Site.Features.BetDetails.ViewModels;

namespace GamesAdmin.Site.Features.BetDetails.Models
{
    public class BetHistoryApiRequest
    {
        public BetHistoryApiRequest(BetDetailsViewModel model)
        {
            Auth = new Auth
            {
                OperatorId = model.OperatorId,
                OperatorSecret = model.OperatorSecret
            };
            GameRoundId = model.RoundId;
            GameTypeId = model.GameType;
            PlayerId = model.PlayerId;
            FreeBet = model.FreeBet;
        }

        public int GameTypeId { get; set; }

        public Auth Auth { get; set; }

        public long GameRoundId { get; set; }

        public int PlayerId { get; set; }

        public bool FreeBet { get; set; }
    }
}