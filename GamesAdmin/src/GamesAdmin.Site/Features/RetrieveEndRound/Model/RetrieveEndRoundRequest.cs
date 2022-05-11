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
                PartnerId = model.OperatorId,
                PartnerSecret = model.OperatorSecret
            };
            GameRoundId = model.RoundId;
            GameTypeId = model.GameType;
            ObCustId = model.PlayerId;
            SiteId = model.SiteId;
            TimeStamp = DateTimeOffset.Now;
            Seq = Guid.NewGuid().ToString();
        }
        public byte GameTypeId { get; set; }

        public Auth Auth { get; set; }

        public long GameRoundId { get; set; }

        public int ObCustId { get; set; }

        public string SiteId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string Seq { get; set; }
    }
}
