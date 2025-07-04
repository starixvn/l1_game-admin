using System.Collections;
using System.Collections.Generic;
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
            IEnumerable<int> otherGameIdsInGroup
            )
        {
            Name = name;
            BotEnabled = botEnabled;
            CascadingSlotsConfig = cascadingSlotsConfig;
            Type = type;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public bool BotEnabled { get; }

        public GameType Type { get; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public IEnumerable<int>  OtherGameIdsInGroup { get; }
    }
}