using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class MiniSeDieReportRequest : BetReportRequest, IRequest<SeDieBetReport>
    {
        public MiniSeDieReportRequest(BetReportFilter filter) : base(filter)
        {

        }
    }
}