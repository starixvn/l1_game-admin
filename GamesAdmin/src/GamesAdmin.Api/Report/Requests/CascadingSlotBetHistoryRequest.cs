using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CascadingSlotBetHistoryRequest : IRequest<CascadingSlotBetHistory>
    {
        public CascadingSlotBetHistoryRequest(CascadingSlotReportFilter filter)
        {
            Filter = filter;
        }
        public CascadingSlotReportFilter Filter { get; }
    }
}
