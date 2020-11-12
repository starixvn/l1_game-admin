namespace GamesAdmin.Core.Models.GameConfigs
{
    public class BotSetting
    {
        public BotSetting(bool botEnabled, double botMaxBet = 10, double[] hoursMaxBot = null)
        {
            BotEnabled = botEnabled;
            BotMaxBet = botMaxBet;
            HoursMaxBot = hoursMaxBot;
        }

        public bool BotEnabled { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }
    }
}