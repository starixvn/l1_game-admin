using GamesAdmin.Api.ChatBot.Results;
using MediatR;

namespace GamesAdmin.Api.ChatBot.Requests
{
    public class NotifyHighStakeRequest : IRequest<ChatBotResult>
    {
        public NotifyHighStakeRequest(int obCustId, byte gameTypeId, double amount)
        {
            ObCustId = obCustId;
            GameTypeId = gameTypeId;
            Amount = amount;
        }

        public int ObCustId { get; set; }

        public byte GameTypeId { get; set; }

        public double Amount { get; set; }
    }
}