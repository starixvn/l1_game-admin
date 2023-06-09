using MediatR;

namespace GamesAdmin.Api.VoidGame.Requests
{
    public class VoidRunningTicketRequest :  IRequest<bool>
    {
        public VoidRunningTicketRequest(string userIdentify)
        {
            this.UserIdentify = userIdentify;
        }

        public string UserIdentify { get; set; }
    }
}
