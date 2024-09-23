using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateGeneralConfigRequest : IRequest<bool>
    {
        public UpdateGeneralConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, OddsMultipliersConfig oddsMultipliers = null, IEnumerable<int> otherGameIdsInGroup = null)
        {
            GameInfo = gameInfo;
            BotSetting = botSetting;
            EnableSetting = enableSetting;
            OddsMultipliers = oddsMultipliers;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public GameInfo GameInfo { get; }

        public BotSetting BotSetting { get; }

        public EnableSetting EnableSetting { get; }

        public OddsMultipliersConfig OddsMultipliers { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}