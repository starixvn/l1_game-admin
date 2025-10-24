using System.Collections;
using System.Collections.Generic;
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
            bool enablePromotionRTP,
            CascadingSlotsConfig cascadingSlotsConfig,
            GameType type,
            IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            DisplayName = displayName;
            BotEnabled = botEnabled;
            EnablePromotionRTP = enablePromotionRTP;
            CascadingSlotsConfig = cascadingSlotsConfig;
            Type = type;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public bool BotEnabled { get; }

        public bool EnablePromotionRTP { get; }

        public GameType Type { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}