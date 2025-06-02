using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class JackpotReportRequest : IRequest<IEnumerable<JackpotRecord>>
    {
        public JackpotReportRequest(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot, string operatorId, IEnumerable<string> siteNames)
        {
            GameId = gameId;
            FromDate = fromDate;
            ToDate = toDate;
            ExcludeUUS = excludeUUS;
            ExcludeBot = excludeBot;
            OperatorId = operatorId;
            SiteNames = siteNames.Where(x => !string.IsNullOrWhiteSpace(x));
        }

        public int GameId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool ExcludeUUS { get; }

        public bool ExcludeBot { get; }

        public string OperatorId { get; }

        public IEnumerable<string> SiteNames { get; }
    }
}