using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateGeneralConfigRequest : IRequest<bool>
    {
        public UpdateGeneralConfigRequest(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, OddsMultipliersConfig oddsMultipliers = null)
        {
            GameInfo = gameInfo;
            BotSetting = botSetting;
            EnableSetting = enableSetting;
            OddsMultipliers = oddsMultipliers;
        }

        public GameInfo GameInfo { get; }

        public BotSetting BotSetting { get; }

        public EnableSetting EnableSetting { get; }

        public OddsMultipliersConfig OddsMultipliers { get; }
    }
}