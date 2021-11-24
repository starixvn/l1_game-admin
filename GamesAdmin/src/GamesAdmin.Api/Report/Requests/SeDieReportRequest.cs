using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class SeDieReportRequest : BetReportRequest, IRequest<SeDieBetReport>
    {
        public SeDieReportRequest(BetReportFilter filter) : base(filter)
        {
        }
    }
}