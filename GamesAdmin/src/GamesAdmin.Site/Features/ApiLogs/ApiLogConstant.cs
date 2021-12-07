using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.ApiLogs.Models;

namespace GamesAdmin.Site.Features.ApiLogs
{
    public static class ApiLogConstant
    {
        private static readonly ApiLogViewConfigs siteIdForPartnerLogConfigs = new ApiLogViewConfigs("Site ID", "Call.SiteId");
        private static readonly ApiLogViewConfigs gameTypeIdForPartnerLogConfigs = new ApiLogViewConfigs("Game", "Call.GameTypeId", GetGameName);
        private static readonly ApiLogViewConfigs gameRoundForPartnerLogIdConfigs = new ApiLogViewConfigs("Game Round", "Call.GameRoundId");
        private static readonly ApiLogViewConfigs requestTypeForPartnerLogIdConfigs = new ApiLogViewConfigs("Request Type", "Call.RequestType");
        private static readonly ApiLogViewConfigs obCustIdForPartnerLogIdConfigs = new ApiLogViewConfigs("Customer ID", "Call.ObCustId");

        private static readonly ApiLogViewConfigs siteIdForSBLogConfigs = new ApiLogViewConfigs("Site ID", "Request.SiteId");
        private static readonly ApiLogViewConfigs gameTypeIdForSBLogConfigs = new ApiLogViewConfigs("Game", "Request.GameTypeId", GetGameName);
        private static readonly ApiLogViewConfigs gameRoundForSBLogIdConfigs = new ApiLogViewConfigs("Game Round", "Request.GameRoundId");
        private static readonly ApiLogViewConfigs obCustIdForSBLogIdConfigs = new ApiLogViewConfigs("Customer ID", "Request.ObCustId");

        private static readonly IEnumerable<ApiLogViewConfigs> commonPartnerLogView = new List<ApiLogViewConfigs>
        {
            siteIdForPartnerLogConfigs, gameTypeIdForPartnerLogConfigs, gameRoundForPartnerLogIdConfigs
        };

        private static readonly IEnumerable<ApiLogViewConfigs> commonSBLogView = new List<ApiLogViewConfigs>
        {
            siteIdForSBLogConfigs, gameTypeIdForSBLogConfigs, gameRoundForSBLogIdConfigs
        };

        private static readonly IEnumerable<ApiLogViewConfigs> signInView = new List<ApiLogViewConfigs>
        {
            obCustIdForSBLogIdConfigs, siteIdForSBLogConfigs
        };

        private static readonly IEnumerable<ApiLogViewConfigs> failRequestsView = new List<ApiLogViewConfigs>
        {
            requestTypeForPartnerLogIdConfigs, obCustIdForPartnerLogIdConfigs, siteIdForPartnerLogConfigs
        };

        private static readonly IEnumerable<ApiLogViewConfigs> voidGameAndTicketDetailView = new List<ApiLogViewConfigs>
        {
            obCustIdForPartnerLogIdConfigs, gameTypeIdForPartnerLogConfigs, gameRoundForPartnerLogIdConfigs
        };

        private static readonly Dictionary<ApiLogType, IEnumerable<ApiLogViewConfigs>> viewConfigsMappings = new Dictionary<ApiLogType, IEnumerable<ApiLogViewConfigs>>
        {
            { ApiLogType.Bets, commonPartnerLogView },
            { ApiLogType.EndGames, commonPartnerLogView },
            { ApiLogType.RetrieveRequests, commonSBLogView },
            { ApiLogType.SignInRequest, signInView },
            { ApiLogType.FailedRequests, failRequestsView },
            { ApiLogType.VoidGames, voidGameAndTicketDetailView },
            { ApiLogType.TicketDetailRequests, voidGameAndTicketDetailView }
        };

        private static string GetGameName(string gameId)
        {
            var game = (GameId)byte.Parse(gameId);
            var gameName = Game.FromGameId(game)?.DisplayName ?? gameId;

            return gameName;
        }

        public static Dictionary<ApiLogType, IEnumerable<ApiLogViewConfigs>> ViewConfigsMappings => viewConfigsMappings;
    }
}