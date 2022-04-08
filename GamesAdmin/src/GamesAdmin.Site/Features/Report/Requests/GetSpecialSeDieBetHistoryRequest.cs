using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSpecialSeDieBetHistoryRequest : IRequest<SpecialSeDieBetHistoryViewModel>
    {
        public GetSpecialSeDieBetHistoryRequest(long roundId, string userId, decimal? rate)
        {
            RoundId = roundId;
            UserId = userId;
            Rate = rate;
        }

        public long RoundId { get; }

        public string UserId { get; }

        public decimal? Rate { get; }
    }
}