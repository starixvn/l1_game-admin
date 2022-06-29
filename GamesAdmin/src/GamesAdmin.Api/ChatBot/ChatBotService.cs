using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const string Separator = " - ";
        private const string HighStakeMessageTemplate = "{0} - {1}";
        private const int MinBetAmount = 2000;

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
            if (request.Currency == "UUS" || request.Amount < MinBetAmount)
            {
                return await Task.FromResult<ChatBotResult>(default);
            }

            var msgBuilder = new StringBuilder();
            var game = Enum.Parse<GameId>(request.GameTypeId.ToString());
            var conversationSetting = appSettings.ChatBotApiSettings.HighStake;
            var charName = await gameUserService.GetCharacterNameAsync(request.ObCustId);

            msgBuilder.Append(string.Format(HighStakeMessageTemplate, charName, request.Amount));
            msgBuilder.Append($"{Separator}{game}");
            msgBuilder.Append(GetSystemName(request.System, request.Metadata));

            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, conversationSetting.ConversationId, msgBuilder.ToString()));

            return await Task.FromResult(new ChatBotResult());
        }

        private static string GetSystemName(byte system, Dictionary<string, string> metadata)
        {
            var systemName = Enum.Parse<Systems>(system.ToString());

            return systemName switch
            {
                Systems.Mobi => $"{Separator}SM",
                Systems.Instant => $"{Separator}SI{GetModeName(metadata)}",
                Systems.Cocos => GetModeName(metadata),
                _ => string.Empty,
            };
        }

        private static string GetModeName(Dictionary<string, string> metadata)
        {
            if(metadata != null && metadata.ContainsKey("mode") && metadata["mode"] == ClientDisplayMode.Portrait.Value)
            {
                return $" - {ClientDisplayMode.Portrait.DisplayName}";
            }

            return string.Empty;
        }
    }
}