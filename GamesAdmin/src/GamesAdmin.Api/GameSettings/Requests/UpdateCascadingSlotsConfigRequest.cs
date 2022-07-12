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
            GameType type)
        {
            Name = name;
            DisplayName = displayName;
            BotEnabled = botEnabled;
            CascadingSlotsConfig = cascadingSlotsConfig;
            Type = type;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public bool BotEnabled { get; }

        public GameType Type { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }
    }
}