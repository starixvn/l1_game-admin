using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetFourGuardiansReportRequest : IRequest<FourGuardiansReportResultViewModel>
    {
        public GetFourGuardiansReportRequest(long roundId, string nickname, bool excludeUUS, string game)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
            Game = Enumeration.FromValue<Game>(game);
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }

        public Game Game { get; }
    }
}