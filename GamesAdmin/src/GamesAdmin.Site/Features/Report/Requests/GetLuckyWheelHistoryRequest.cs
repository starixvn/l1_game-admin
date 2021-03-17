using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetLuckyWheelHistoryRequest : IRequest<LuckyWheelHistoryViewModel>
    {
        public GetLuckyWheelHistoryRequest(string nickname)
        {
            Nickname = nickname;
        }

        public string Nickname { get; }
    }
}