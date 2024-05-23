using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(
            GameInfo gameInfo,
            BotSetting botSetting,
            EnableSetting enableSetting,
            SlotsConfig slotsConfig,
            CascadingSlotsConfig cascadingSlotsConfig,
            SlotRNGConfig slotRNGConfig,
            CrashConfig crashConfig,
            OddsMultipliersConfig oddsMultipliers)
        {
            GameInfo = gameInfo;
            BotSetting = botSetting;
            EnableSetting = enableSetting;
            SlotsConfig = slotsConfig;
            CascadingSlotsConfig = cascadingSlotsConfig;
            SlotRNGConfig = slotRNGConfig;
            CrashConfig = crashConfig;
            OddsMultipliers = oddsMultipliers;
        }

        public GameInfo GameInfo { get; }

        public BotSetting BotSetting { get; }

        public EnableSetting EnableSetting { get; }

        public SlotsConfig SlotsConfig { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public SlotRNGConfig SlotRNGConfig { get; }

        public CrashConfig CrashConfig { get; }

        public OddsMultipliersConfig OddsMultipliers { get; set; }
    }
}