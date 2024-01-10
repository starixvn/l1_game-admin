using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class SetPontoonResultRequest : IRequest<bool>
    {
        public string name { get; set; }

        public BlackjackCard[] dealer { get; set; }

        public Hands[] hands { get; set; }
    }

    public class Hands
    {
        public byte pos { get; set; }

        public BlackjackCard[] cards { get; set; }
    }

    public class BlackjackCard
    {
        public string rank { get; set; }

        public byte suit { get; set; }
    }
}
