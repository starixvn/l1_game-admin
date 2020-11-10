namespace GamesAdmin.Core.Models.GameConfigs
{
    public abstract class BaseConfig
    {
        protected BaseConfig(
           string id,
           GameInfo info,
           EnableSettings enableSettings,
           BotSettings botSettings)
        {
            Id = id;
            Name = info.Name;
            DisplayName = info.DisplayName;
            EnableFreeBet = info.EnableFreeBet;
            Type = info.Type;

            Enabled = enableSettings.Enabled;
            DisabledRound = enableSettings.DisabledRound;
            DisabledMessage = enableSettings.DisabledMessage;
            DelayStartTime = enableSettings.DelayStartTime;

            BotEnabled = botSettings.BotEnabled;
            BotMaxBet = botSettings.BotMaxBet;
            HoursMaxBot = botSettings.HoursMaxBot;
        }

        public string Id { get; }

        public byte Type { get; }

        public string Name { get; }

        public string DisplayName { get; }

        public bool EnableFreeBet { get; }

        public bool Enabled { get; }

        public string DisabledMessage { get; }

        public int? DisabledRound { get; }

        public bool BotEnabled { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }

        public int DelayStartTime { get; }
    }

    public class GameInfo
    {
        public GameInfo(
            byte type, 
            string name, 
            string displayName, 
            bool freeBet) 
        {
            Type = type;
            Name = name;
            DisplayName = displayName;
            EnableFreeBet = freeBet;
        }

        public byte Type { get; }

        public string Name { get; }

        public string DisplayName { get; }

        public bool EnableFreeBet { get; }
    }

    public class EnableSettings
    {
        public EnableSettings(bool enabled, string disabledMessage, int? disabledRound, int delayStartTime) 
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

    public class BotSettings
    {
        public BotSettings(bool enabled, double maxBet, double[] hoursMaxBot) 
        {
            BotEnabled = enabled;
            BotMaxBet = maxBet;
            HoursMaxBot = hoursMaxBot;
        }

        public bool BotEnabled { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }
    }
}
