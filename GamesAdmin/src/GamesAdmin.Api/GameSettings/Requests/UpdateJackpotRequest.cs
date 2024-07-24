using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateJackpotRequest : IRequest<bool>
    {
        public UpdateJackpotRequest(string name, Jackpot jackpot, double[] betLevels)
        {
            Name = name;
            Jackpot = jackpot;
            BetLevels = betLevels;
        }

        public string Name { get; }

        public Jackpot Jackpot { get; }

        public double[] BetLevels { get; }
    }
}
