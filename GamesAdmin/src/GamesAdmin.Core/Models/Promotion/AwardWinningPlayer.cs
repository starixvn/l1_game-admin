using System;

namespace GamesAdmin.Core.Models.Promotion
{
    public class AwardWinningPlayer
    {
        public decimal Amount { get; set; }

        public int PromotionId { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public string PlayerId { get; set; }

        public string SiteId { get; set; }

        public int GameTypeId { get; set; }

        public string PlayerName { get; set; }

        public string Nickname { get; set; }

        public string SiteName { get; set; }

        public double Point { get; set; }

        public bool IsTestCurrency { get; set; }

        public int Ranking { get; set; }

        public bool CompletedPromotion { get; set; }
        public bool IsAwarded { get; set; }
        public DateTime AwardedTime { get; set; }
        public double AwardedAmount { get; set; }
    }
}