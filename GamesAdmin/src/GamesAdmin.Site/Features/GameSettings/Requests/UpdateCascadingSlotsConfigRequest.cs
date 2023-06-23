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
            GameType type)
        {
            Name = name;
            BotEnabled = botEnabled;
            CascadingSlotsConfig = cascadingSlotsConfig;
            Type = type;
        }

        public string Name { get; }

        public bool BotEnabled { get; }

        public GameType Type { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }
    }
}