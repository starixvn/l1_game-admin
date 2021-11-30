using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class ReportRequest : IRequest<ReportResultViewModel>
    {
        public ReportRequest(GameId game, BetReportFilter filter)
        {
            Game = game;
            Filter = filter;
        }

        public BetReportFilter Filter { get; }
        public GameId Game { get; }
    }
}