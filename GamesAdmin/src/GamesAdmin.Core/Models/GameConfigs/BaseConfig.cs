namespace GamesAdmin.Core.Models.GameConfigs
{
    public abstract class BaseConfig
    {
        protected BaseConfig(
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
        {
            Id = id;
            Name = name;
            Enabled = enabled;
            BotEnabled = botEnabled;
            DisabledRound = disabledround;
            BotMaxBet = botMaxBet;
            HoursMaxBot = hoursMaxBot;
            DisabledMessage = disabledMessage;
            DelayStartTime = delayStartTime;
            EnableFreeBet = enableFreeBet;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public bool BotEnabled { get; }

        public int? DisabledRound { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }

        public string DisabledMessage { get; }

        public int DelayStartTime { get; }

        public bool EnableFreeBet { get; }
    }
}
