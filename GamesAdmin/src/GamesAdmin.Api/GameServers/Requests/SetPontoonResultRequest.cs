using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetPontoonResultRequest : IRequest<bool>
    {
        public string name { get; set; }

        public PokerCard[] dealer { get; set; }

        public Hands[] hands { get; set; }
    }

    public class Hands
    {
        public string pos { get; set; }

        public PokerCard[] cards { get; set; }
    }
}
