using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSlotRNGConfigRequest : UpdateGeneralConfigRequest, IRequest<bool>
    {
        public UpdateSlotRNGConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, SlotRNGConfig slotRNGConfig) : base(gameInfo, botSetting, enableSetting)
        {
            SlotRNGConfig = slotRNGConfig;
        }

        public SlotRNGConfig SlotRNGConfig { get; }
    }
}
