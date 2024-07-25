using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class AwardTestCurrencyRequest : AwardTournamentRequest, IRequest<bool>
    {
        public AwardTestCurrencyRequest(int promotionId, string playerId, string operatorId, double awardAmount) : base(promotionId)
        {
            PlayerId = playerId;
            OperatorId = operatorId;
            AwardAmount = awardAmount;
        }
        public string PlayerId { get;  }
        public string OperatorId { get;  }
        public double AwardAmount { get;  }
    }
}