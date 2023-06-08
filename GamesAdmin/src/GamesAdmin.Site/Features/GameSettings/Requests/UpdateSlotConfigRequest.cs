using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSlotConfigRequest : IRequest<bool>
    {
        public UpdateSlotConfigRequest(
            string name,
            bool botEnabled,
            SlotsConfig slotsConfig,
            GameType type,
            bool isMicrosite)
        {
            Name = name;
            BotEnabled = botEnabled;
            SlotsConfig = slotsConfig;
            Type = type;
            IsMicrosite = isMicrosite;
        }

        public string Name { get; }

        public bool BotEnabled { get; }

        public SlotsConfig SlotsConfig { get; }

        public GameType Type { get; }

        public bool IsMicrosite { get; }
    }
}