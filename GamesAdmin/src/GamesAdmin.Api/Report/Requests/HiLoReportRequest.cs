using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class HiLoReportRequest : IRequest<IEnumerable<HiLoBetRecord>>
    {
        public HiLoReportRequest(HiLoBetReportFilter filter)
        {
            Filter = filter;
        }

        public HiLoBetReportFilter Filter { get; }
    }
}