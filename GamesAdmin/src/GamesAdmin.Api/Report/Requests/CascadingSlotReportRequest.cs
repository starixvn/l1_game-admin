using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CascadingSlotReportRequest : IRequest<IEnumerable<CascadingSlotBetRecord>>
    {
        public CascadingSlotReportRequest(CascadingSlotReportFilter filter)
        {
            Filter = filter;
        }

        public CascadingSlotReportFilter Filter { get; }
    }
}