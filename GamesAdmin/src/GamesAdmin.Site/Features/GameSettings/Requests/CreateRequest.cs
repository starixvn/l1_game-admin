using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(AutoRNGGameConfig game, SlotsConfigRequest slotsConfig)
        {
            Game = game;
            SlotConfigs = slotsConfig;
        }

        public AutoRNGGameConfig Game { get; }

        public SlotsConfigRequest SlotConfigs { get; }
    }

    public class SlotsConfigRequest
    {
        public SlotsConfigRequest(
            bool enableJackpot,
            byte numberOfIcons,
            SlotDifficulties difficulty,
            double minBonus,
            double maxBonus,
            List<StakeLevel> stakes)
        {
            EnableJackpot = enableJackpot;
            NumberOfIcons = numberOfIcons;
            Difficulty = difficulty;
            MinBonus = minBonus;
            MaxBonus = maxBonus;
            Stakes = stakes;
        }

        public bool EnableJackpot { get; set; }

        public byte NumberOfIcons { get; set; }

        public SlotDifficulties Difficulty { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        public List<StakeLevel> Stakes { get; set; }
    }
}
