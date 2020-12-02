using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateGeneralSettingRequest : IRequest<bool>
    {
        public UpdateGeneralSettingRequest(string name, GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting)
        {
            Name = name;
            GameInfo = gameInfo;
            BotSetting = botSetting;
            EnableSetting = enableSetting;
        }

        public string Name { get; }

        public GameInfo GameInfo { get; }

        public BotSetting BotSetting { get; }

        public EnableSetting EnableSetting { get; }
    }
}