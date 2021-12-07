using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetLotteryBetHistoryRequest : IRequest<LotteryBetHistoryViewModel>
    {
        public GetLotteryBetHistoryRequest(long roundId, string userId, bool convertToUSD)
        {
            RoundId = roundId;
            UserId = userId;
            ConvertToUSD = convertToUSD;
        }

        public long RoundId { get; }

        public string UserId { get; }

        public bool ConvertToUSD { get; }
    }
}