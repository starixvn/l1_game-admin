using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSlotConfigRequest : IRequest<bool>
    {
        public UpdateSlotConfigRequest(string name, bool botEnabled, SlotsConfig slotsConfig)
        {
            Name = name;
            BotEnabled = botEnabled;
            SlotsConfig = slotsConfig;
        }

        public string Name { get; }

        public bool BotEnabled { get; }

        public SlotsConfig SlotsConfig { get; }
    }
}