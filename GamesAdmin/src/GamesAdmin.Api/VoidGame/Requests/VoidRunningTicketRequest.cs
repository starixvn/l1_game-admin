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

        public string UserIdentify { get; set; }

        public string Name { get; set; }
    }
}
