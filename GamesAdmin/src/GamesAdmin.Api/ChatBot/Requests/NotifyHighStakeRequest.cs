using GamesAdmin.Api.ChatBot.Results;
using MediatR;

namespace GamesAdmin.Api.ChatBot.Requests
{
    public class NotifyHighStakeRequest : IRequest<ChatBotResult>
    {
        public NotifyHighStakeRequest(int obCustId, byte gameTypeId, double amount, string currency, int choiceId, byte system)
        {
            ObCustId = obCustId;
            GameTypeId = gameTypeId;
            Amount = amount;
            Currency = currency;
            ChoiceId = choiceId;
            System = system;
        }

        public int ObCustId { get; set; }

        public byte GameTypeId { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public int ChoiceId { get; set; }

        public byte System { get; set; }
    }
}