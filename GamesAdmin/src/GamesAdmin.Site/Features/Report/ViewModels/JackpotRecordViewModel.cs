using GamesAdmin.Core.Models.GameConfigs;
using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class JackpotRecordViewModel
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Pool { get; set; }

        public long RoundId { get; set; }

        public decimal WinningAmount { get; set; }

        public int ExchangeRateId { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public decimal Rate { get; set; }

        public StakeLevel[] BaseStakeLevel { get; set; }

        public StakeLevel[] StakeLevelByCurrency { get; set; }

        public void ConvertToUserCurrency(bool isConvert, int baseCurrencyMultiplier = 1)
        {
            if (isConvert)
            {
                WinningAmount /= (decimal)Rate / baseCurrencyMultiplier;
                Pool = ConvertPool();
            }
        }

        private decimal ConvertPool()
        {
            var indexToGetValue = Array.FindIndex(BaseStakeLevel, stakeLevel => (decimal)stakeLevel.Stake == Pool);
            return IsIndexValid(indexToGetValue, StakeLevelByCurrency.Length)
                   ? (decimal)StakeLevelByCurrency[indexToGetValue].Stake
                   : Pool;
        }

        private static bool IsIndexValid(int index, int arrayLength)
        {
            return index >= 0 && index < arrayLength;
        }
    }
}