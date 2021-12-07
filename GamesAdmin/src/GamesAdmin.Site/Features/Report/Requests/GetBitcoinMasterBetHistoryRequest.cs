using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBitcoinMasterBetHistoryRequest : IRequest<BitcoinMasterBetHistoryViewModel>
    {
        public GetBitcoinMasterBetHistoryRequest(long roundId, string nickname, string currency)
        {
            RoundId = roundId;
            Nickname = nickname;
            Currency = currency;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public string Currency { get; }
    }
}