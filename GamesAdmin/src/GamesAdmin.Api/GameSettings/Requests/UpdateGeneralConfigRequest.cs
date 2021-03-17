using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateGeneralConfigRequest : IRequest<bool>
    {
        public UpdateGeneralConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting)
        {
            GameInfo = gameInfo;
            BotSetting = botSetting;
            EnableSetting = enableSetting;
        }

        public GameInfo GameInfo { get; }

        public BotSetting BotSetting { get; }

        public EnableSetting EnableSetting { get; }
    }
}