using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateJackpotRequest : IRequest<bool>
    {
        public UpdateJackpotRequest(string name, Jackpot jackpot, List<StakeLevel> stakeLevels)
        {
            Name = name;
            Jackpot = jackpot;
            StakeLevels = stakeLevels;
        }

        public string Name { get; }

        public Jackpot Jackpot { get; }

        public List<StakeLevel> StakeLevels { get; }
    }
}
