using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSlotBotConfigRequest : IRequest<bool>
    {
        public UpdateSlotBotConfigRequest(
            string name,
            Jackpot jackpot,
            double[] betLevels
           )
        {
            Name = name;
            Jackpot = jackpot;
            BetLevels = betLevels;
        }

        public string Name { get; set; }

        public Jackpot Jackpot { get; set; }

        public double[] BetLevels { get; set;}
    }
}