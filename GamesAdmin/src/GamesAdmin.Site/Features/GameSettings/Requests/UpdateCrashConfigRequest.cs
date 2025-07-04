using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateCrashConfigRequest : UpdateGeneralConfigRequest, IRequest<bool>
    {
        public UpdateCrashConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, CrashConfig crashConfig, IEnumerable<int> otherGameIdsInGroup) : base(gameInfo, botSetting, enableSetting)
        {
            CrashConfig = crashConfig;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public CrashConfig CrashConfig { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}
