using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class PartnerCall : IApiLogRequest
    {
        public string Seq { get; set; }

        public string PlayerId { get; set; }

        public string SiteId { get; set; }

        public int? GameRoundId { get; set; }

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

        public string OperatorToken { get; set; }
    }

    public class PartnerVoidGameCall : PartnerCall
    {
        public string Reason { get; set; }
    }
}