using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(
            GameInfo gameInfo,
            EnableSetting enableSetting,
            BotSetting botSettings,
            BetLimit betLimit,
            SlotsConfig slotsConfig,
            CascadingSlotsConfig cascadingSlotsConfig)
        {
            GameInfo = gameInfo;
            EnableSetting = enableSetting;
            BotSettings = botSettings;
            BetLimit = betLimit;
            SlotConfigs = slotsConfig;
            CascadingSlotsConfig = cascadingSlotsConfig;
        }

        public GameInfo GameInfo { get; }

        public EnableSetting EnableSetting { get; }

        public BotSetting BotSettings { get; }

        public BetLimit BetLimit { get; }

        public SlotsConfig SlotConfigs { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }
    }
}