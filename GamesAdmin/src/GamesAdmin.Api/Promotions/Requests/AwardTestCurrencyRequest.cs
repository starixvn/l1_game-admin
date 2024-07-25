using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class AwardTestCurrencyRequest: AwardTournamentRequest,IRequest<bool>
    {
        public string PlayerId { get; set; }
        public string OperatorId { get; set; }
        public double AwardAmount { get; set; }
    }
}