using System;
using System.Globalization;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class JackpotRecordViewModel
    {
        public DateTime Time { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public decimal Pool { get; set; }
        public string Prize { get; set; }
        public string DisplayPrize =>  string.IsNullOrWhiteSpace(Prize) ? Pool.ToString() : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Prize);

        public long RoundId { get; set; }

        public decimal WinningAmount { get; set; }

        public int ExchangeRateId { get; set; }

        public string Currency { get; set; }

        public string OperatorId { get; set; }

        public string OperatorName { get; set; }
        public string SiteName { get; set; }

        public decimal Rate { get; set; }

        public double[] BaseBetLevel { get; set; }

        public double[] BetLevelByCurrency { get; set; }

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
            var indexToGetValue = Array.FindIndex(BaseBetLevel, betLevel => (decimal)betLevel == Pool);
            return IsIndexValid(indexToGetValue, BetLevelByCurrency.Length)
                   ? (decimal)BetLevelByCurrency[indexToGetValue]
                   : Pool;
        }

        private static bool IsIndexValid(int index, int arrayLength)
        {
            return index >= 0 && index < arrayLength;
        }
    }
}