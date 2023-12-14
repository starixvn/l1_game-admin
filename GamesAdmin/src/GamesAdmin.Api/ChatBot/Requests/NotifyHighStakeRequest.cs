using System.Collections.Generic;
using GamesAdmin.Api.ChatBot.Results;
using MediatR;

namespace GamesAdmin.Api.ChatBot.Requests
{
    public class NotifyHighStakeRequest : IRequest<ChatBotResult>
    {
        public NotifyHighStakeRequest(
            string obCustId,
            int gameTypeId,
            double amount,
            string currency,
            int choiceId,
            byte system,
            string operatorId,
            string userId)
        {
            ObCustId = obCustId;
            GameTypeId = gameTypeId;
            Amount = amount;
            Currency = currency;
            ChoiceId = choiceId;
            System = system;
            OperatorId = operatorId;
            UserId = userId;
        }

        public string ObCustId { get; set; }

        public int GameTypeId { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public int ChoiceId { get; set; }

        public byte System { get; set; }

        public string OperatorId { get; set; }

        public string UserId { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}