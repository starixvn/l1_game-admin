using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(
            GameInfo gameInfo, 
            EnableSettings enableSettings,
            BotSettings botSettings,
            BetLimit betLimit,
            SlotsConfigRequest slotsConfig)
        {
            GameInfo = gameInfo;
            EnableSettings = enableSettings;
            BotSettings = botSettings;
            BetLimit = betLimit;
            SlotConfigs = slotsConfig;
        }

        public GameInfo GameInfo { get; }

        public EnableSettings EnableSettings { get; }

        public BotSettings BotSettings { get; }

        public BetLimit BetLimit { get; }

        public SlotsConfigRequest SlotConfigs { get; }
    }

    public class SlotsConfigRequest
    {
        public SlotsConfigRequest(
            bool enableJackpot,
            byte numberOfIcons,
            SlotDifficulties difficulty,
            double minBonus,
            double maxBonus)
        {
            EnableJackpot = enableJackpot;
            NumberOfIcons = numberOfIcons;
            Difficulty = difficulty;
            MinBonus = minBonus;
            MaxBonus = maxBonus;
        }

        public bool EnableJackpot { get; set; }

        public byte NumberOfIcons { get; set; }

        public SlotDifficulties Difficulty { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        public List<StakeLevel> Stakes { get; set; }
    }
}