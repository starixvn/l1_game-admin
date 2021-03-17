namespace GamesAdmin.Core.Models.GameConfigs
{
    public class BotSetting
    {
        private const byte MaxHours = 24;

        public BotSetting(bool botEnabled, double botMaxBet = 10, double[] hoursMaxBot = null)
        {
            BotEnabled = botEnabled;
            BotMaxBet = botMaxBet;
            HoursMaxBot = hoursMaxBot?.Length == 0 ? GenerateDefaultMaxBot() : hoursMaxBot;
        }

        public bool BotEnabled { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }

        private double[] GenerateDefaultMaxBot()
        {
            var maxBots = new double[24];

            for (var i = 0; i < MaxHours; i++)
            {
                maxBots[i] = 0;
            }

            return maxBots;
        }
    }
}