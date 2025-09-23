using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class PartnerCall : IApiLogRequest
    {
        public string Seq { get; set; }

        public object PlayerId { get; set; }

        public string SiteId { get; set; }

        public long? GameRoundId { get; set; }

        public int? GameTypeId { get; set; }

        public string RequestType { get; set; }

        public string OperatorToken { get; set; }
    }

    public class PartnerEndGameCall : PartnerCall
    {
        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ValidBetAmount { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public EndGameMetaData MetaData { get; set; }

        public object PromotionId { get; set; }
    }

    public class EndGameMetaData
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

        public decimal ValidBetAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }
    }

    public class PartnerEnterPortalCall : PartnerCall
    {
        public string CharacterName { get; set; }

        public bool FirstLogin { get; set; }
    }

    public class PartnerPlaceBetCall : PartnerCall
    {
        public int ChoiceId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTimeOffset? RoundEndTime { get; set; }

        public string Ip { get; set; }

        public bool FreeBet { get; set; }

        public bool FreeSpin { get; set; }

        public float ContributionRate { get; set; }

        public decimal ContributionAmount { get; set; }

        public object PromotionId { get; set; }
    }

    public class PartnerVoidGameCall : PartnerCall
    {
        public string Reason { get; set; }
    }

    public class PartnerAwardCall : PartnerCall
    {
        public string Currency { get; set; }
        public object PromotionId { get; set; }
        public decimal Amount { get; set; }
    }

    public class PartnerGetBetLimitCall : PartnerCall
    {
    }

    public class PartnerHitJackpotCall : PartnerCall
    {
        public decimal WinningAmount { get; set; }
    }
}