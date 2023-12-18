using System;

namespace GamesAdmin.Site.Features.ApiLogs.Models
{
    public class QueryLogModel
    {
        public QueryLogModel(
            byte logType,
            string partnerId,
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            string obCustId,
            int gameRoundId,
            int gameTypeId,
            string username,
            string siteId)
        {
            LogType = logType;
            PartnerId = partnerId;
            FromDate = fromDate;
            ToDate = toDate;
            ObCustId = obCustId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Username = username;
            SiteId = siteId;
        }

        public byte LogType { get; }

        public string PartnerId { get; }

        public DateTimeOffset FromDate { get; }

        public DateTimeOffset ToDate { get; }

        public string ObCustId { get; }

        public int GameRoundId { get; }

        public int GameTypeId { get; }
        public string Username { get; }

        public string SiteId { get; }
    }
}