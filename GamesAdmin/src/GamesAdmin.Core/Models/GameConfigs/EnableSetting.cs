namespace GamesAdmin.Core.Models.GameConfigs
{
    public class EnableSetting
    {
        private const string DefaultDisabledMessage = "Game đang được bảo trì, vui lòng quay lại sau 10 phút.";

        public EnableSetting(bool enabled, int delayStartTime = 0, int? disabledRound = 0, string disabledMessage = DefaultDisabledMessage)
        {
            Enabled = enabled;
            DisabledMessage = disabledMessage;
            DisabledRound = disabledRound;
            DelayStartTime = delayStartTime;
        }

        public bool Enabled { get; }

        public string DisabledMessage { get; }

        public int? DisabledRound { get; }

        public int DelayStartTime { get; }
    }
}