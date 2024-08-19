using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBotSlotSettingsRequest : IRequest<bool>
    {
        public UpdateBotSlotSettingsRequest(string name, Jackpot jackpot, double[] betLevels)
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