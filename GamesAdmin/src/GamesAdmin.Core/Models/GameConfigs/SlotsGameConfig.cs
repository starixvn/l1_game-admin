using Newtonsoft.Json;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class SlotsGameConfig : BaseConfig
    {
        [JsonConstructor]
        public SlotsGameConfig(
            string id,
            string name,
            bool enabled,
            bool botEnabled,
            int? disabledround,
            double botMaxBet,
            double[] hoursMaxBot,
            string disabledMessage,
            int delayStartTime,
            bool enableFreeBet)
            : base(
               id,
               name,
               enabled,
               botEnabled,
               disabledround,
               botMaxBet,
               hoursMaxBot,
               disabledMessage,
               delayStartTime,
               enableFreeBet)
        {

        }
    }
}
