using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBotSlotSettingsRequest : IRequest<bool>
    {
        public UpdateBotSlotSettingsRequest(string name, Jackpot jackpot, double[] betLevels, IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            Jackpot = jackpot;
            BetLevels = betLevels;
            OtherGameIdsInGroup= otherGameIdsInGroup;
        }

        public string Name { get; }

        public Jackpot Jackpot { get; }

        public double[] BetLevels { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}