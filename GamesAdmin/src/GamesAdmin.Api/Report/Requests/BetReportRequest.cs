using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Api.Report.Requests
{
    public class BetReportRequest
    {
        public BetReportRequest(BetReportFilter filter)
        {
            Filter = filter;
        }

        public BetReportFilter Filter { get; }
    }
}
