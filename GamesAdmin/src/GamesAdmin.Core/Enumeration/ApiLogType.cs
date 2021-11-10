namespace GamesAdmin.Core.Enumeration
{
    public enum ApiLogType
    {
        Bets = 1,
        EndGames,
        FailedRequests,
        RetrieveRequests,
        SignInRequest,
        TicketDetailRequests, // TODO
        VoidGames,
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

        public ApiLogEnumeration()
        { }

        public ApiLogEnumeration(string displayName, ApiLogType type)
            : base(type.ToString(), displayName)
        {
            Type = type;
        }

        public ApiLogType Type { get; }
    }
}