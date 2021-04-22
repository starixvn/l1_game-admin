using System;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using GamesAdmin.Api.GameUser;
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
        private const string HighStakeMessageTemplate = "{0} - {1}{2} - {3}";

        private readonly IAppSettings appSettings;
        private readonly IChatBotApi chatBotApi;
        private readonly IGameUserService gameUserService;

        public ChatBotService(IAppSettings appSettings, IChatBotApi chatBotApi, IGameUserService gameUserService)
        {
            this.appSettings = appSettings;
            this.chatBotApi = chatBotApi;
            this.gameUserService = gameUserService;
        }

        public async Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request)
        {
            if (request.Currency == "UUS")
            {
                return await Task.FromResult<ChatBotResult>(default);
            }

            var game = Enum.Parse<GameId>(request.GameTypeId.ToString());
            var conversationSetting = appSettings.ChatBotApiSettings.HighStake;
            var charName = await gameUserService.GetCharacterNameAsync(request.ObCustId);
            var message = string.Format(HighStakeMessageTemplate, charName, request.Amount, request.Currency, game.ToString());

            return await chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, conversationSetting.ConversationId, message);
        }
    }
}