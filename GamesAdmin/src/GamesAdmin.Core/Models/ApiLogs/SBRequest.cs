using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class SBRequest : IApiLogRequest
    {
        public string Seq { get; set; }

        public string SiteId { get; set; }

        public int? GameRoundId { get; set; }

        public int? GameTypeId { get; set; }

        public int? ObCustId { get; set; }

        public int? PlayerId { get; set; }

        public virtual string RequestType { get; set; }
    }

    public class Auth
    {
        public string OperatorId { get; set; }

        public string OperatorSecret { get; set; }
    }

    public class SignInRequest : SBRequest
    {
        public string Domain { get; set; }

        public string BrowserUserAgent { get; set; }

        public string Currency { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public string MemberName { get; set; }

        public string PlayerName { get; set; }

        public byte AppType { get; set; }

        public string SiteName { get; set; }

        public Auth Auth { get; set; }

        public bool DisableJackpotGames { get; set; }

        public string OperatorToken { get; set; }

        public string Ref { get; set; }

        public string ReturnUrl { get; set; }

        public bool DisableAward { get; set; }
    }

    public class EndGameInfoRequest : SBRequest
    {
        public DateTimeOffset TimeStamp { get; set; }

        public Auth Auth { get; set; }

        public bool FreeBet { get; set; }
    }

    public class TicketDetailRequest : SBRequest
    {
        public Auth Auth { get; set; }

        public string Language { get; set; }

        public bool FreeBet { get; set; }

        public override string RequestType => nameof(TicketDetailRequest);
    }
}