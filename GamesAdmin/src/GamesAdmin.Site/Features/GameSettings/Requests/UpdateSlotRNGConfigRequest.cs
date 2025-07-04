using System.Collections;
using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSlotRNGConfigRequest : UpdateGeneralConfigRequest, IRequest<bool>
    {
        public UpdateSlotRNGConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, SlotRNGConfig slotRNGConfig, IEnumerable<int> otherGameIdsInGroup):
            base(gameInfo, botSetting, enableSetting)
        {
            SlotRNGConfig = slotRNGConfig;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public SlotRNGConfig SlotRNGConfig { get; }
        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}
