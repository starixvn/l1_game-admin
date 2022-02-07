using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetHoldemPokerReportRequest : IRequest<HoldemPokerReportResultViewModel>
    {
        private readonly Dictionary<GameId, string> holdemPokerBetHistoryUrls = new Dictionary<GameId, string>
        {
            { GameId.CasinoHoldemPoker, "CasinoHoldemPokerBetHistory" },
            { GameId.TexasHoldemPoker, "TexasHoldemPokerBetHistory" },
        };

        public GetHoldemPokerReportRequest(HoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public HoldemPokerBetReportFilter Filter { get; }

        public string BetHistoryUrl => holdemPokerBetHistoryUrls[Filter.GameId];
    }
}