namespace GamesAdmin.Core.Models.ApiLogs
{
    public interface IPartnerResult : IApiLogResult
    {
        string Seq { get; }

        int ErrorCode { get; }

        string ErrorMessage { get; }

        string ErrorDescription { get; }

        object PlayerId { get; }
    }

    public class PartnerResult : IPartnerResult
    {
        public string Seq { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDescription { get; set; }

        public string ObCustId { get; set; }

        public object PlayerId { get; set; }

        public string ResultType { get; set; }
    }

    public class PartnerEnterPortalResult : PartnerResult
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }
    }

    public class PartnerPlaceBetResult : PartnerResult
    {
        public long? ObTransId { get; set; }

        public decimal Balance { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }
    }

    public class PartnerGetBetLimitResult : PartnerResult
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }
    }
}