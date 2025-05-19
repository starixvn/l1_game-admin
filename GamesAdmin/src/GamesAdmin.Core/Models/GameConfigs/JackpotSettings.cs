using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class JackpotSettings
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }

        public int GameId { get; set; }

        public string OperatorId { get; set; }

        public string[] SiteNames { get; set; }

        public bool ExcludeTestCurrency { get; set; }

        public double WinningRatio { get; set; }

        public JackpotPools Pools { get; set; }

        public double[] BotBetLevels { get; set; }

        public bool BotEnabled { get; set; }

        public double[] HoursMaxBot { get; set; }
    }

    public class UpdateJackpotSettingsResult
    {
        public int Code { get; set; }
        public string Message { get; set; } = "Success";
        public bool Success => Code == 0;

        public string Id { get; set; }

        public static UpdateJackpotSettingsResult SuccessResult(string id)
        {
            return new UpdateJackpotSettingsResult { Id = id, Code = 0 };
        }

        public static UpdateJackpotSettingsResult FailedResult(string message = "Failed")
        {
            return new UpdateJackpotSettingsResult { Code = 1, Message = message };
        }

        public static UpdateJackpotSettingsResult ExistedSiteResult(string gameName, string operatorName, IEnumerable<string> siteNames)
        {
            return new UpdateJackpotSettingsResult { 
                Code = 2,
                Message = $"Site Name '{string.Join(", ", siteNames)}' already exists for {gameName} - {operatorName}"
            };
        }
    }
}