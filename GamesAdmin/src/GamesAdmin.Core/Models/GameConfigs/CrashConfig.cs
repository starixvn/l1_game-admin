using GamesAdmin.Core.Enumeration.CrashGame;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class CrashConfig
    {
        public int MaxMultiplier { get; set; } = 0;

        public int MaxWinPerRound { get; set; } = 0;

        public double CrashAtBase { get; set; } = 0;

        public MultiplierConfig MultiplierConfigs { get; set; }

        public List<FixedStepsMultiplierConfig> FixedStepsMultiplierConfigs { get; set; }
    }

    public class FixedStepsMultiplierConfig
    {
        public byte Mode { get; set; }
        public string ModeName
        {
            get => GameModeHelper.GetModeName(Mode);
        }
        
        public FixedStepsMultiplierConfig(GameMode gameMode)
        {
            Mode = (byte)gameMode;
            MultiplierSteps = new List<double>();
        }

        public List<double> MultiplierSteps { get; set; }

        /// <summary>
        /// Gets or sets the Values list as a semicolon-separated string for form binding
        /// </summary>
        [BsonIgnore]
        public string ValuesString
        {
            get => MultiplierSteps != null ? string.Join(";", MultiplierSteps) : string.Empty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MultiplierSteps = new List<double>();
                }
                else
                {
                    MultiplierSteps = value.Split(';')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => double.TryParse(x.Trim(), out double val) ? val : 0)
                        .ToList();
                }
            }
        }
    }

    public class MultiplierConfig
    {
        public List<MultiplierStage> Stages { get; set; }

        public double InitialMultiplier { get; set; }
    }

    public class MultiplierStage
    {
        public double MinMultiplier { get; set; }
        public double MaxMultiplier { get; set; }
        public double Increment { get; set; }
    }
}
