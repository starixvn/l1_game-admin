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
        private const string TicketDetailCollectionName = "ticket_detail_requests";
        private const string MemberSigninCollectionName = "membersignin_requests";
        private const string OperatorSigninCollectionName = "owsignin_requests";
        private const string HitJackpotCollectionName = "hit_jackpots";
        private const string EnterPortalCollectionName = "enter_portals";
        private const string GetBetLimitCollectionName = "bet_limits";
        private const string AwardCollectionName = "awards";

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
        private const string RetrieveEndRoundInfoRequest = "RetrieveEndRoundInfoRequest";
        private const string EndRoundInfoResult = "EndRoundInfoResult";

        private const string EnterPortalCall = "PartnerEnterPortalCall";
        private const string EnterPortalResult = "PartnerEnterPortalResult";

        private const string GetBetLimitCall = "PartnerGetBetLimitCall";
        private const string GetBetLimitResult = "PartnerGetBetLimitResult";

        private const string AwardTournamentCall = "PartnerAwardCall";

        private const string HitJackpotCall = "HitJackpotCall";


        private const string TicketDetailCall = nameof(TicketDetailRequest);
        private const string TicketDetailResultName = nameof(TicketDetailResult);

        public const string MemberSignInCall = nameof(MemberSignInCall);
        public const string MemberSignInResult = nameof(MemberSignInResult);
        public const string OWSignInCall = nameof(OWSignInCall);
        public const string OWSignInResult = nameof(OWSignInResult);

        private static readonly Dictionary<ApiLogType, string> collectionMappings = new Dictionary<ApiLogType, string>
        {
            { ApiLogType.Bets, BetsCollectionName },
            { ApiLogType.FailedRequests, FailedRequestsCollectionName },
            { ApiLogType.EndGames, EndGamesCollectionName },
            { ApiLogType.VoidGames, VoidGamesCollectionName },
            { ApiLogType.SignInRequest, SignInCollectionName },
            { ApiLogType.RetrieveRequests, RetrieveEndGameCollectionName },
            { ApiLogType.TicketDetailRequests, TicketDetailCollectionName },
            { ApiLogType.MemberSiteSignInRequest, MemberSigninCollectionName },
            { ApiLogType.OperatorSignInRequest, OperatorSigninCollectionName },
            { ApiLogType.HitJackpot, HitJackpotCollectionName },
            { ApiLogType.EnterPortal, EnterPortalCollectionName },
            { ApiLogType.GetBetLimit, GetBetLimitCollectionName },
            { ApiLogType.Awards, AwardCollectionName },
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
            { RetrieveEndRoundInfoRequest, typeof(EndGameInfoRequest) },
            { EndRoundInfoResult, typeof(RetrieveEndGameInfoResult) },

            { EnterPortalCall, typeof(PartnerEnterPortalCall) },
            { EnterPortalResult, typeof(PartnerEnterPortalResult) },

            { TicketDetailCall, typeof(TicketDetailRequest) },
            { TicketDetailResultName, typeof(TicketDetailResult) },

            { MemberSignInCall, typeof(MemberFailedSignInRequest) },
            { MemberSignInResult, typeof(MemberFailedSignInResult) },

            { OWSignInCall, typeof(OWFailedSignInCall) },
            { OWSignInResult, typeof(OWFailedSignInResult) },

            { GetBetLimitCall, typeof(PartnerGetBetLimitCall) },
            { GetBetLimitResult, typeof(PartnerGetBetLimitResult) },

            { AwardTournamentCall, typeof(PartnerAwardCall) },

            { HitJackpotCall, typeof(PartnerHitJackpotCall) },
        };

        public static Dictionary<ApiLogType, string> CollectionMappings => collectionMappings;

        public static Dictionary<string, Type> CallOrRequestTypeMapping => callOrRequestTypeMapping;
    }
}