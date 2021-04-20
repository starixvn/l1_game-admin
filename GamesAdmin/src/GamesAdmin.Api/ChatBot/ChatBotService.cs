using System;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using GamesAdmin.Core.Enumeration;
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
            var game = Enum.Parse<GameId>(request.GameTypeId.ToString());
            var conversationSetting = appSettings.ChatBotApiSettings.HighStake;
            var message = string.Format(conversationSetting.MessageTemplate, game.ToString());

            return chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, conversationSetting.ConversationId, message);
        }
    }
}