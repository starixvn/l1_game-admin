using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.RoundResults;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetCasinoHoldemPokerResultRequest : IRequest<CasinoHoldemPokerResultControl>
    {
        public string roundName { get; set; }

        public PokerCard[] player { get; set; }

        public PokerCard[] dealer { get; set; }

        public PokerCard[] communityCards { get; set; }
    }
}