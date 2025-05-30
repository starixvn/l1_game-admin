using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class JackpotPoolsReportRequest : IRequest<IEnumerable<JackpotPool>>
    {
        public JackpotPoolsReportRequest(int gameId, string operatorId, string siteNames)
        {
            GameId = gameId;
            OperatorId = operatorId;
            SiteNames = string.IsNullOrWhiteSpace(siteNames) ? Enumerable.Empty<string>() : siteNames.Split(';').Select(x => x.Trim());
        }

        public int GameId { get; }

        public string OperatorId { get; }

        public IEnumerable<string> SiteNames { get; }
    }
}
