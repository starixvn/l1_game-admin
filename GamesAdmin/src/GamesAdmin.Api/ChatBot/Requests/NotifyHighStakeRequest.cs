using GamesAdmin.Api.ChatBot.Results;
using MediatR;

namespace GamesAdmin.Api.ChatBot.Requests
{
    public class NotifyHighStakeRequest : IRequest<ChatBotResult>
    {
        public NotifyHighStakeRequest(int obCustId, byte gameTypeId, decimal amount)
        {
            ObCustId = obCustId;
            GameTypeId = gameTypeId;
            Amount = amount;
        }

        public int ObCustId { get; set; }

        public byte GameTypeId { get; set; }

        public decimal Amount { get; set; }
    }
}