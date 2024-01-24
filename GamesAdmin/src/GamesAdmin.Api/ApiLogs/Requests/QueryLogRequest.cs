using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.ApiLogs;
using MediatR;

namespace GamesAdmin.Api.ApiLogs.Requests
{
    public class QueryLogModel
    {
        public byte LogType { get; set; }

        public string PartnerId { get; set; }

        public DateTimeOffset FromDate { get; set; }

        public DateTimeOffset ToDate { get; set; }

        public int ObCustId { get; set; }

        public int GameRoundId { get; set; }

        public int GameTypeId { get; set; }

        public string Username { get; set; }

        public string SiteId { get; set; }  
    }

    public class QueryLogRequest : IRequest<IEnumerable<IApiLog>>
    {
        public QueryLogRequest(ApiLogType logType, string partnerId, DateTimeOffset fromDate, DateTimeOffset toDate, int playerId, int gameRoundId, int gameTypeId, string username, string siteId)
        {
            LogType = logType;
            PartnerId = partnerId;
            FromDate = fromDate;
            ToDate = toDate;
            PlayerId = playerId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Username = username;
            SiteId = siteId;
        }

        public ApiLogType LogType { get; }

        public string PartnerId { get; }

        public DateTimeOffset FromDate { get; }

        public DateTimeOffset ToDate { get; }

        public int PlayerId { get; }

        public int GameRoundId { get; }

        public int GameTypeId { get; }

        public string Username { get; set; }

        public string SiteId { get; }
    }
}