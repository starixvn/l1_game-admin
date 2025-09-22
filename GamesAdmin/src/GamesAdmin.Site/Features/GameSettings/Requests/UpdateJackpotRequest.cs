using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSlotBotConfigRequest : IRequest<bool>
    {
        public UpdateSlotBotConfigRequest(
            string name,
            Jackpot jackpot,
            double[] betLevels,
            IEnumerable<int> otherGameIdsInGroup
           )
        {
            Name = name;
            Jackpot = jackpot;
            BetLevels = betLevels;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; set; }

        public Jackpot Jackpot { get; set; }

        public double[] BetLevels { get; set;}


        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}