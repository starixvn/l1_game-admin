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
            int obCustId,
            int gameRoundId,
            int gameTypeId,
            string username)
        {
            LogType = logType;
            PartnerId = partnerId;
            FromDate = fromDate;
            ToDate = toDate;
            ObCustId = obCustId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Username = username;
        }

        public byte LogType { get; }

        public string PartnerId { get; }

        public DateTimeOffset FromDate { get; }

        public DateTimeOffset ToDate { get; }

        public int ObCustId { get; }

        public int GameRoundId { get; }

        public int GameTypeId { get; }
        public string Username { get; }
    }
}