using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.ApiLogs;

namespace GamesAdmin.Api.ApiLogs
{
    public static class ApiLogConstant
    {
        private const string FailedRequestsCollectionName = "failed_requests";
        private const string BetsCollectionName = "bets";
        private const string EndGamesCollectionName = "end_games";
        private const string VoidGamesCollectionName = "void_games";
        private const string SignInCollectionName = "signin_requests";
        private const string RetrieveEndGameCollectionName = "retrieve_requests";
        private const string TicketDetaulCollectionName = "ticket_detail_requests";

        private const string PlaceBetCall = "PartnerPlaceBetCall";
        private const string PlaceBetResult = "PartnerPlaceBetResult";

        private const string EndGameCall = "PartnerEndGameCall";
        private const string EndGameResult = "PartnerEndGameResult";

        private const string VoidGameCall = "PartnerVoidGameCall";
        private const string VoidGameResult = "PartnerVoidGameResult";

        private const string SignInRequest = "SignIn";
        private const string SignInResult = "SingleSignOnResult";

        private const string RetrieveEndGameRequest = "EndGameInfoRequest";
        private const string RetrieveEndGameResult = "RetrieveEndGameInfoResult";

        private const string EndRoundInfoRequest = "EndRoundInfoRequest";
        private const string EndRoundInfoResult = "EndRoundInfoResult";

        private const string EnterPortalCall = "PartnerEnterPortalCall";
        private const string EnterPortalResult = "PartnerEnterPortalResult";

        private const string TicketDetailCall = nameof(TicketDetailRequest);
        private const string TicketDetailResultName = nameof(TicketDetailResult);

        private static readonly Dictionary<ApiLogType, string> collectionMappings = new Dictionary<ApiLogType, string>
        {
            { ApiLogType.Bets, BetsCollectionName },
            { ApiLogType.FailedRequests, FailedRequestsCollectionName },
            { ApiLogType.EndGames, EndGamesCollectionName },
            { ApiLogType.VoidGames, VoidGamesCollectionName },
            { ApiLogType.SignInRequest, SignInCollectionName },
            { ApiLogType.RetrieveRequests, RetrieveEndGameCollectionName },
            { ApiLogType.TicketDetailRequests, TicketDetaulCollectionName }
        };

        private static readonly Dictionary<string, Type> callOrRequestTypeMapping = new Dictionary<string, Type>
        {
            { PlaceBetCall, typeof(PartnerPlaceBetCall) },
            { PlaceBetResult, typeof(PartnerPlaceBetResult) },

            { EndGameCall, typeof(PartnerEndGameCall) },
            { EndGameResult, typeof(PartnerResult) },

            { VoidGameCall, typeof(PartnerVoidGameCall) },
            { VoidGameResult, typeof(PartnerResult) },

            { SignInRequest, typeof(SignInRequest) },
            { SignInResult, typeof(SingleSignOnResult) },

            { RetrieveEndGameRequest, typeof(EndGameInfoRequest) },
            { RetrieveEndGameResult, typeof(RetrieveEndGameInfoResult) },
            { EndRoundInfoRequest, typeof(EndGameInfoRequest) },
            { EndRoundInfoResult, typeof(RetrieveEndGameInfoResult) },

            { EnterPortalCall, typeof(PartnerEnterPortalCall) },
            { EnterPortalResult, typeof(PartnerEnterPortalResult) },

            { TicketDetailCall, typeof(TicketDetailRequest) },
            { TicketDetailResultName, typeof(TicketDetailResult) }
        };

        public static Dictionary<ApiLogType, string> CollectionMappings => collectionMappings;

        public static Dictionary<string, Type> CallOrRequestTypeMapping => callOrRequestTypeMapping;
    }
}