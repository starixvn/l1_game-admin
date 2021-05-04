namespace GamesAdmin.Core.Models.GameConfigs
{
    public class BotSetting
    {
        private const byte TotalHoursInDay = 24;

        public BotSetting(
            bool botEnabled, 
            double botMaxBet = 10, 
            double[] hoursMaxBot = null,
            bool smartBotEnabled = false, 
            double smartBotGroupRatio = 0 )
        {
            BotEnabled = botEnabled;
            BotMaxBet = botMaxBet;
            HoursMaxBot = hoursMaxBot?.Length == 0 ? GenerateDefaultBotRatioPerHour() : hoursMaxBot;
            SmartBotEnabled = smartBotEnabled;
            SmartBotGroupRatio = smartBotGroupRatio;
        }

        public bool BotEnabled { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }

        private static double[] GenerateDefaultBotRatioPerHour()
        {
            var botRatio = new double[24];

            for (var i = 0; i < TotalHoursInDay; i++)
            {
                botRatio[i] = 0;
            }

            return botRatio;
        }

        public bool SmartBotEnabled { get; }

        public double SmartBotGroupRatio { get; }
    }
}