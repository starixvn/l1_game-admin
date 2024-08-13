using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateJackpotRequest : IRequest<bool>
    {
        public UpdateJackpotRequest(
            string name,
            Jackpot jackpot,
            List<StakeLevel> stakeLevels
           )
        {
            Name = name;
            Jackpot = jackpot;
            StakeLevels = stakeLevels;
        }

        public string Name { get; set; }

        public Jackpot Jackpot { get; set; }

        public List<StakeLevel> StakeLevels { get; set; }

    }
}