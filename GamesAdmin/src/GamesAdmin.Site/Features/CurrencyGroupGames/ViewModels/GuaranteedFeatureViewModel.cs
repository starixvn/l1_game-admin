using System.Collections.Generic;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels
{
    public class GuaranteedFeatureViewModel
    {
        public byte GamingDays { get; set; }
        public byte TotalDays {get; set; }
        public GuaranteedConfig GuaranteedWin { get; set; } = new GuaranteedConfig();
        public GuaranteedConfig GuaranteedLoss { get; set; } = new GuaranteedConfig();
    }

    public class ThresholdSetting
    {
        public double Threshold { get; set; }
        public float NormalWinningPercentage { get; set; }
        public bool EnabledWeakened { get; set; }
        public float WeakenedWinningPercentage { get; set; }
    }

    public class GuaranteedConfig
    {
        public bool Enabled { get; set; }
        public byte Floating { get; set; }
        public List<ThresholdSetting> ThresholdSettings { get; set; } = new List<ThresholdSetting>();
    }
}
