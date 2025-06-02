using System;
using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class JackpotRunningPoolsReportRequest : IRequest<IEnumerable<JackpotPool>>
    {
        public JackpotRunningPoolsReportRequest(int gameId, string operatorId, IEnumerable<string> siteNames)
        {
            GameId = gameId;
            OperatorId = operatorId;
            SiteNames = siteNames?.Where(x => !string.IsNullOrWhiteSpace(x)) ?? Enumerable.Empty<string>();
        }

        public int GameId { get; }

        public string OperatorId { get; }

        public IEnumerable<string> SiteNames { get; }
    }
}