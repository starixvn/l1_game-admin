using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBitcoinMasterBetHistoryRequest : IRequest<BitcoinMasterBetHistoryViewModel>
    {
        public GetBitcoinMasterBetHistoryRequest(long roundId, string nickname, decimal? rate)
        {
            RoundId = roundId;
            Nickname = nickname;
            Rate = rate;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public decimal? Rate { get; }
    }
}