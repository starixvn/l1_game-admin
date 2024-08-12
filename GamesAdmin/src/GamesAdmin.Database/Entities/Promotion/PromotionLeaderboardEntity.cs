using System;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("promotion_leaderboards")]
    public class PromotionLeaderboardEntity : Document
    {
        public int PromotionId { get; set; }
        public string OperatorId { get; set; }
        public string CurrencyGroupId { get; set; }
        public string Currency { get; set; }
        public bool IsTestCurrency { get; set; }
        public string SiteName { get; set; }
        public string SiteId { get; set; }
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string NickName { get; set; }
        public double PointInBaseCurrency { get; set; }
        public double AwardedAmount { get; set; }
        public DateTime DataSourceTimeStamp { get; set; }
        public bool IsAwarded { get; set; }
        public DateTime AwardedTime { get; set; }
    }
}