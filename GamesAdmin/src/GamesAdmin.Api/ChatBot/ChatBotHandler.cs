using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using MediatR;

namespace GamesAdmin.Api.ChatBot
{
    public class ChatBotHandler : IRequestHandler<NotifyHighStakeRequest, ChatBotResult>
    {
        private readonly IChatBotService chatBotService;

        public ChatBotHandler(IChatBotService chatBotService)
        {
            this.chatBotService = chatBotService;
        }

        public Task<ChatBotResult> Handle(NotifyHighStakeRequest request, CancellationToken cancellationToken)
        => chatBotService.NotifyHighStake(request);
    }
}