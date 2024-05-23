using System.Collections.Generic;
using System.ComponentModel;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class OddsMultipliersConfig
    {
        [DisplayName("Extra Fee Ratio")]
        public double ExtraFeeRatio { get; set; }
        public NumberOfCardsRange NumberOfCardsRange { get; set; } = new NumberOfCardsRange();
        public List<MultiplierDistribution> Values { get; set; }
    }

    public class NumberOfCardsRange
    {
        [DisplayName("From")]
        public int Min { get; set; }

        [DisplayName("To")]
        public int Max { get; set; }
    }

    public class MultiplierDistribution
    {
        public double Number { get; set; }
        public double Ratio { get; set; }
    }
}