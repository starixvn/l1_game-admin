using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class HoldemPokerBetHistoryRequest : IRequest<HoldemPokerBetHistory>
    {
        public HoldemPokerBetHistoryRequest(HoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }


        public HoldemPokerBetHistoryFilter Filter { get; }
    }
}
