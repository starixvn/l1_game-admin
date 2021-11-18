using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBigSmallTurboReportRequest : IRequest<BigSmallTurboReportResultViewModel>
    {
        public GetBigSmallTurboReportRequest(long roundId, string nickname, bool excludeBot, string partnerId, string username, string currency)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeBot = excludeBot;
            PartnerId = partnerId;
            Username = username;
            Currency = currency;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeBot { get; }

        public string PartnerId { get; }

        public string Username { get; }

        public string Currency { get; }
    }
}