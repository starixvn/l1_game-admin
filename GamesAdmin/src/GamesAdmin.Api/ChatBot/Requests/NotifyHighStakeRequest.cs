using GamesAdmin.Api.ChatBot.Results;
using MediatR;

namespace GamesAdmin.Api.ChatBot.Requests
{
    public class NotifyHighStakeRequest : IRequest<ChatBotResult>
    {
        public NotifyHighStakeRequest(int obCustId, byte gameTypeId)
        {
            ObCustId = obCustId;
            GameTypeId = gameTypeId;
        }

        public int ObCustId { get; set; }

        public byte GameTypeId { get; set; }
    }
}