namespace GamesAdmin.Site.Features.RetrieveEndRound.Model
{
    using System;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;

    public class RetrieveEndRoundApiRequest : BetHistoryApiRequest
    {
        public RetrieveEndRoundApiRequest(BetDetailsViewModel model)
             : base(model)
        {
            TimeStamp = DateTimeOffset.Now;
            Seq = Guid.NewGuid().ToString();
        }

        public DateTimeOffset TimeStamp { get; set; }

        public string Seq { get; set; }
    }
}