using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class BetHistoryRequest : IRequest<BetHistory>
    {
        public BetHistoryRequest(BetHistoryFilter filter)
        {
            Filter = filter;
        }

        public BetHistoryFilter Filter { get; }
    }
}