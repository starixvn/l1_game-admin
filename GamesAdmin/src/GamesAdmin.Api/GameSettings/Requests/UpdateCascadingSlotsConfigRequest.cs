using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateCascadingSlotsConfigRequest : IRequest<bool>
    {
        public UpdateCascadingSlotsConfigRequest(
            string name,
            string displayName,
            bool botEnabled,
            CascadingSlotsConfig cascadingSlotsConfig,
            GameType type,
            bool isMicrosite)
        {
            Name = name;
            DisplayName = displayName;
            BotEnabled = botEnabled;
            CascadingSlotsConfig = cascadingSlotsConfig;
            Type = type;
            IsMicrosite = isMicrosite;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public bool BotEnabled { get; }

        public GameType Type { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public bool IsMicrosite { get; }
    }
}