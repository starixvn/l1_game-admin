using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(AutoRNGGameConfig game)
        {
            Game = game;
        }

        public AutoRNGGameConfig Game { get; }

        public AddSlotsConfigRequest SlotConfigs { get; set; }
    }

    public class AddSlotsConfigRequest
    {
        public bool EnableJackpot { get; set; }

        public byte NumberOfIcons { get; set; }

        public SlotDifficulties Difficulty { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        public List<StakeLevel> Stakes { get; set; }
    }
}
