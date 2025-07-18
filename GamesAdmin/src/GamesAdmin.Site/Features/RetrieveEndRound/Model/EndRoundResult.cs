using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GamesAdmin.Site.Features.RetrieveEndRound.Model
{
    public class EndRoundResult
    {
        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ValidBetAmount { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public MetaData MetaData { get; set; }
    }

    public class MetaData
    {
        public List<BetDetail> BetDetails { get; set; }

        public string Result { get; set; }

        public string GuaranteedWinStatus { get; set; }

        public bool GuaranteedWinTriggered { get; set; }

        public string GuaranteedLossStatus { get; set; }

        public bool GuaranteedLossTriggered { get; set; }
    }

    public class BetDetail
    {
        public int ChoiceId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal ValidBetAmount { get; set; }

        public decimal TotalWin { get; set; }
    }
}
