using System;
using System.Collections.Generic;
using GamesAdmin.Database.ApiLogs.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    public class PartnerLogEntity : ApiLogEntity
    {
        public string PartnerId { get; set; }

        public string OperatorId { get; set; }

        public PartnerCallEntity Call { get; set; }

        public PartnerResultEntity Result { get; set; }

        public byte System { get; set; }

        public string AccessKey { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }

    public class PartnerCallEntity : ApiLogRequestEntity
    {
        public string Seq { get; set; }

        public string SiteId { get; set; }

        public int ChoiceId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTimeOffset? RoundEndTime { get; set; }

        public string Ip { get; set; }

        public bool FreeBet { get; set; }

        public bool FreeSpin { get; set; }

        public float ContributionRate { get; set; }

        public decimal ContributionAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ValidBetAmount { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public string CharacterName { get; set; }

        public bool FirstLogin { get; set; }

        public string Reason { get; set; }

        public string OperatorToken { get; set; }

        public string VendorId { get; set; }

        public string Username { get; set; }

        public string Platform { get; set; }

        public string ObCustName { get; set; }
        public int GameId { get; set; }

        public string IP { get; set; }

        public string BrowserUserAgent { get; set; }

        public string BetFrom { get; set; }

        public string Language { get; set; }

        public string SiteName { get; set; }

        public int PromotionId { get; set; }
    }

    public class PartnerResultEntity : ApiLogResultEntity
    {
        public string Seq { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDescription { get; set; }

        public string ObCustId { get; set; }

        [BsonSerializer(typeof(PlayerIdSerializer))]
        public object PlayerId { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public long? ObTransId { get; set; }

        public decimal Balance { get; set; }

        public string ErrorMsg { get; set; }
    }
}