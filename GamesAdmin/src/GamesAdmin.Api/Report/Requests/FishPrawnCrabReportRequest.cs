using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class FishPrawnCrabReportRequest: BetReportRequest, IRequest<FishPrawnCrabBetReport>
    {
        public FishPrawnCrabReportRequest(BetReportFilter filter) : base (filter)
        {
       
        }

    
    }
}
