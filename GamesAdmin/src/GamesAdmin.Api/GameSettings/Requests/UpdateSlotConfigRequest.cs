using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSlotConfigRequest : IRequest<bool>
    {
        public UpdateSlotConfigRequest(
            string name,
            string displayName,
            bool botEnabled,
            SlotsConfig slotsConfig,
            GameType type,
            bool isMicrosite)
        {
            Name = name;
            DisplayName = displayName;
            BotEnabled = botEnabled;
            SlotsConfig = slotsConfig;
            Type = type;
            IsMicrosite = isMicrosite;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public bool BotEnabled { get; }

        public SlotsConfig SlotsConfig { get; }

        public GameType Type { get; set; }

        public bool IsMicrosite { get; }
    }
}