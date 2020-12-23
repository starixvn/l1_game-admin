using System;
using System.Collections.Generic;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetJackpotReportRequest : IRequest<IEnumerable<JackpotRecordViewModel>>
    {
        public GetJackpotReportRequest(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
        {
            GameId = gameId;
            FromDate = fromDate;
            ToDate = toDate;
            ExcludeUUS = excludeUUS;
            ExcludeBot = excludeBot;
        }

        public byte GameId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool ExcludeUUS { get; }

        public bool ExcludeBot { get; }
    }
}