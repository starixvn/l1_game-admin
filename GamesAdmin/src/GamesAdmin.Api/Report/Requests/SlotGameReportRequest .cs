using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class SlotGameReportRequest : IRequest<IEnumerable<SlotGameBetRecord>>
    {
        public SlotGameReportRequest(SlotGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public SlotGameBetReportFilter Filter { get; }
    }
}