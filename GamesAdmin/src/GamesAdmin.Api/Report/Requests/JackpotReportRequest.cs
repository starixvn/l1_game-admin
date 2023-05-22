using System;
using System.Collections.Generic;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class JackpotReportRequest : IRequest<IEnumerable<JackpotRecord>>
    {
        public JackpotReportRequest(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
        {
            GameId = gameId;
            FromDate = fromDate;
            ToDate = toDate;
            ExcludeUUS = excludeUUS;
            ExcludeBot = excludeBot;
        }

        public int GameId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool ExcludeUUS { get; }

        public bool ExcludeBot { get; }
    }
}