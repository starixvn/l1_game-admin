using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateCrashConfigRequest : UpdateGeneralConfigRequest, IRequest<bool>
    {
        public UpdateCrashConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, CrashConfig crashConfig) : base(gameInfo, botSetting, enableSetting)
        {
            CrashConfig = crashConfig;
        }

        public CrashConfig CrashConfig { get; }
    }
}
