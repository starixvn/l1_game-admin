using System.Collections.Generic;
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
            bool enablePromotionRTP,
            SlotsConfig slotsConfig,
            GameType type, 
            IEnumerable<int> otherGameIdsInGroup = null
            )
        {
            Name = name;
            DisplayName = displayName;
            BotEnabled = botEnabled;
            EnablePromotionRTP = enablePromotionRTP;
            SlotsConfig = slotsConfig;
            Type = type;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public bool BotEnabled { get; }

        public bool EnablePromotionRTP { get; }

        public SlotsConfig SlotsConfig { get; }

        public GameType Type { get; set; }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}