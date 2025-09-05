using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<GameCreationResult>
    {
        public CreateRequest(
            GameInfo gameInfo,
            EnableSetting enableSetting,
            BotSetting botSettings,
            BetLimit betLimit,
            SlotsConfig slotsConfig,
            CascadingSlotsConfig cascadingSlotsConfig,
            SlotRNGConfig slotRNGConfig,
            Dictionary<string, double> odds = null,
            Dictionary<string, string> oddsDisplayNames = null,
            List<Choice> choices = null)
        {
            GameInfo = gameInfo;
            EnableSetting = enableSetting;
            BotSettings = botSettings;
            BetLimit = betLimit;
            SlotConfigs = slotsConfig;
            CascadingSlotsConfig = cascadingSlotsConfig;
            SlotRNGConfig = slotRNGConfig;
            Odds = odds ?? new Dictionary<string, double>();
            OddsDisplayNames = oddsDisplayNames ?? new Dictionary<string, string>();
            Choices = choices ?? new List<Choice>();
        }

        public GameInfo GameInfo { get; }

        public EnableSetting EnableSetting { get; }

        public BotSetting BotSettings { get; }

        public BetLimit BetLimit { get; }

        public SlotsConfig SlotConfigs { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public SlotRNGConfig SlotRNGConfig { get; }

        public Dictionary<string, double> Odds { get; }

        public Dictionary<string, string> OddsDisplayNames { get; }

        public List<Choice> Choices { get; }


    }
}