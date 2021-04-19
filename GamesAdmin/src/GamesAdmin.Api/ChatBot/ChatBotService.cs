using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using Refit;

namespace GamesAdmin.Api.ChatBot
{
    public interface IChatBotApi
    {
        [Post("/forwardwithtoken")]
        Task<ChatBotResult> ForwardMessage(string token, string conversationId, string message);
    }

    public interface IChatBotService
    {
        Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request);
    }

    public class ChatBotService : IChatBotService
    {
        private readonly IAppSettings appSettings;
        private readonly IChatBotApi chatBotApi;

        public ChatBotService(IAppSettings appSettings, IChatBotApi chatBotApi)
        {
            this.appSettings = appSettings;
            this.chatBotApi = chatBotApi;
        }

        public Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request)
        {
            var conversationSetting = appSettings.ChatBotApiSettings.HighStake;
            var message = string.Format(conversationSetting.MessageTemplate, request.GameTypeId);

            return chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, conversationSetting.ConversationId, message);
        }
    }
}