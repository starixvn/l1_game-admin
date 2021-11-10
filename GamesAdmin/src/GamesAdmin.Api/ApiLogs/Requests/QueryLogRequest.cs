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

        public byte GameTypeId { get; set; }
    }

    public class QueryLogRequest : IRequest<IEnumerable<IApiLog>>
    {
        public QueryLogRequest(ApiLogType logType, string partnerId, DateTimeOffset fromDate, DateTimeOffset toDate, int obCustId, int gameRoundId, byte gameTypeId)
        {
            LogType = logType;
            PartnerId = partnerId;
            FromDate = fromDate;
            ToDate = toDate;
            ObCustId = obCustId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
        }

        public ApiLogType LogType { get; }

        public string PartnerId { get; }

        public DateTimeOffset FromDate { get; }

        public DateTimeOffset ToDate { get; }

        public int ObCustId { get; }

        public int GameRoundId { get; }

        public byte GameTypeId { get; }
    }
}