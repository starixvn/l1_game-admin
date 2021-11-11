using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public interface ISBResult : IApiLogResult
    {
        int Code { get; }

        string Message { get; }
    }

    public class SBResult : ISBResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public virtual string ResultType { get; set; }
    }

    public class SingleSignOnResult : SBResult
    {
        public string RedirectUrl { get; set; }
    }

    public class RetrieveEndGameInfoResult : SBResult
    {
        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ValidBetAmount { get; set; }

        public string Seq { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }
    }

    public class TicketDetailResult : SBResult
    {
        public string RedirectUrl { get; set; }

        public string Error { get; set; }

        public override string ResultType => nameof(TicketDetailResult);
    }
}