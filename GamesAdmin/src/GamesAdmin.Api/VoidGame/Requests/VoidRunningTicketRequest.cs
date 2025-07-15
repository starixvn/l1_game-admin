using MediatR;

namespace GamesAdmin.Api.VoidGame.Requests
{
    public class VoidRunningTicketRequest :  IRequest<bool>
    {
        public VoidRunningTicketRequest(string userIdentify, string name)
        {
            this.UserIdentify = userIdentify;
            this.Name = name;
        }

        public VoidRunningTicketRequest(string userIdentify, string name, long roundId)
        {
            UserIdentify = userIdentify;
            Name = name;
            RoundId = roundId;
        }

        public string UserIdentify { get; set; }

        public string Name { get; set; }

        public long RoundId { get; set; }
    }
}
