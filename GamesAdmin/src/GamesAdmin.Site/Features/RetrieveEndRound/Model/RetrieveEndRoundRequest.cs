namespace GamesAdmin.Site.Features.RetrieveEndRound.Model
{
    using System;
    using GamesAdmin.Core.Models.ApiLogs;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;

    public class RetrieveEndRoundRequest
    {
        public RetrieveEndRoundRequest(BetDetailsViewModel model)
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
            TimeStamp = DateTimeOffset.Now;
            Seq = Guid.NewGuid().ToString();
        }
        public int GameTypeId { get; set; }

        public Auth Auth { get; set; }

        public long GameRoundId { get; set; }

        public int PlayerId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string Seq { get; set; }

        public bool FreeBet { get; set; }
    }
}
