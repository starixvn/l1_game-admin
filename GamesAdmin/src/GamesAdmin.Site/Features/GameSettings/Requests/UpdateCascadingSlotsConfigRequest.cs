using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateCascadingSlotsConfigRequest : IRequest<bool>
    {
        public UpdateCascadingSlotsConfigRequest(
            string name,
            bool botEnabled,
            CascadingSlotsConfig cascadingSlotsConfig,
            GameType type,
            bool isMicrosite)
        {
            Name = name;
            BotEnabled = botEnabled;
            CascadingSlotsConfig = cascadingSlotsConfig;
            Type = type;
            IsMicrosite = isMicrosite;
        }

        public string Name { get; }

        public bool BotEnabled { get; }

        public GameType Type { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public bool IsMicrosite { get; }
    }
}