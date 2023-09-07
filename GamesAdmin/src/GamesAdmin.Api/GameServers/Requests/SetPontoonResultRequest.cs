using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetPontoonResultRequest : IRequest<bool>
    {
        public string name { get; set; }

        public PontoonCard[] dealer { get; set; }

        public Hands[] hands { get; set; }
    }

    public class Hands
    {
        public byte pos { get; set; }

        public PontoonCard[] cards { get; set; }
    }

    public class PontoonCard
    {
        public string rank { get; set; }

        public byte suit { get; set; }
    }
}
