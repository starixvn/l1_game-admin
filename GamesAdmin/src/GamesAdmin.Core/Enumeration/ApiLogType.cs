namespace GamesAdmin.Core.Enumeration
{
    public enum ApiLogType
    {
        Bets = 1,
        EndGames,
        FailedRequests,
        RetrieveRequests,
        SignInRequest,
        TicketDetailRequests,
        VoidGames,
        MemberSiteSignInRequest,
        OperatorSignInRequest,
        HitJackpot,
        EnterPortal,
        GetBetLimit
    }

    public class ApiLogEnumeration : Enumeration
    {
        public static readonly ApiLogEnumeration Bets = new ApiLogEnumeration("Bets", ApiLogType.Bets);
        public static readonly ApiLogEnumeration EndGames = new ApiLogEnumeration("End Games", ApiLogType.EndGames);
        public static readonly ApiLogEnumeration FailedRequests = new ApiLogEnumeration("Failed Requests", ApiLogType.FailedRequests);
        public static readonly ApiLogEnumeration RetrieveRequests = new ApiLogEnumeration("Retrieve Requests", ApiLogType.RetrieveRequests);
        public static readonly ApiLogEnumeration SignInRequest = new ApiLogEnumeration("Sign In Request", ApiLogType.SignInRequest);
        public static readonly ApiLogEnumeration TicketDetailRequests = new ApiLogEnumeration("Ticket Detail Requests", ApiLogType.TicketDetailRequests);
        public static readonly ApiLogEnumeration VoidGames = new ApiLogEnumeration("Void Games", ApiLogType.VoidGames);
        public static readonly ApiLogEnumeration MemberSiteSignInRequest = new ApiLogEnumeration("Member Site Sign In Request", ApiLogType.MemberSiteSignInRequest);
        public static readonly ApiLogEnumeration OperatorSignInRequest = new ApiLogEnumeration("Operator Sign In Request", ApiLogType.OperatorSignInRequest);
        public static readonly ApiLogEnumeration HitJackpotRequest = new ApiLogEnumeration("Hit Jackpot", ApiLogType.HitJackpot);
        public static readonly ApiLogEnumeration EnterPortalRequest = new ApiLogEnumeration("Enter Portal", ApiLogType.EnterPortal);
        public static readonly ApiLogEnumeration GetBetLimitRequest = new ApiLogEnumeration("Get Bet Limit", ApiLogType.GetBetLimit);

        public ApiLogEnumeration()
        { }

        public ApiLogEnumeration(string displayName, ApiLogType type)
            : base(type.ToString(), displayName)
        {
            Type = type;
        }

        public ApiLogType Type { get; }

        public static bool IsRelatedToGame(ApiLogType apiLogType)
            => apiLogType == ApiLogType.Bets
            || apiLogType == ApiLogType.EndGames
            || apiLogType == ApiLogType.FailedRequests
            || apiLogType == ApiLogType.HitJackpot
            || apiLogType == ApiLogType.TicketDetailRequests
            || apiLogType == ApiLogType.VoidGames;
    }
}