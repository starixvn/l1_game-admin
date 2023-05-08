using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class CascadingSlotRecordViewModel : BetRecordViewModel
    {
        public long RoundId { get; set; }

        public int ExchangeRateId { get; set; }

        public decimal TotalAmount { get; set; }

        public bool Paid { get; set; }

        public string Game { get; set; }

        public byte BetLevel { get; set; }

        public decimal CoinValue { get; set; }

        public byte TotalMultiplier { get; set; }

        public byte Multiplier { get; set; }

        public decimal TotalTumbleWin { get; set; }

        public bool FreeSpin { get; set; }

        public string Result { get; set; }

        public decimal WinLoss => FreeSpin ? BetWin : BetWin - TotalAmount;

        public IEnumerable<byte> Board => Result.Split(',').Select(value => byte.Parse(value));

        public bool ReachMaxWin { get; set; }

        public void ConvertToUSD(bool isConvert, int baseCurrencyMultiplier = 1)
        {
            if (!isConvert)
            {
                return;
            }

            TotalAmount *= Rate / baseCurrencyMultiplier;
            BetWin *= Rate / baseCurrencyMultiplier;
            CoinValue *= Rate / baseCurrencyMultiplier;
            TotalTumbleWin *= Rate / baseCurrencyMultiplier;

            if (SettlementResult != null && SettlementResult.Tumbles.Any())
            {
                foreach (var tumble in SettlementResult.Tumbles)
                {
                    tumble.Win *= Rate / baseCurrencyMultiplier;
                    if (tumble.Icons != null)
                    {
                        foreach (var (key, _) in tumble.Icons)
                        {
                            tumble.Icons[key].Win *= Rate / baseCurrencyMultiplier;
                        }
                    }
                }
            }
        }

        public CascadingSlotSettlementResultViewModel SettlementResult { get; set; }
    }

    public class CascadingSlotRoundMultiplierViewModel
    {
        public DateTimeOffset Time { get; set; }

        public long RoundId { get; set; }

        public byte Multiplier { get; set; }

        public byte TotalMultiplier { get; set; }
    }

    public class CascadingSlotSettlementResultViewModel
    {
        public IEnumerable<CascadingSlotTumbleViewModel> Tumbles { get; set; }

        public SlotGameFreeSpinViewModel FreeSpins { get; set; }
    }

    public class CascadingSlotTumbleViewModel
    {
        public Dictionary<byte, CascadingSlotWinningSymbolViewModel> Icons { get; set; }

        public decimal Win { get; set; }

        public string OriginalBoard { get; set; }

        public IEnumerable<byte> Multipliers { get; set; }

        public IEnumerable<byte> Board => OriginalBoard.Split(',').Select(value => byte.Parse(value));
    }

    public class CascadingSlotWinningSymbolViewModel
    {
        public byte Count { get; set; }

        public decimal Odds { get; set; }

        public decimal Win { get; set; }
    }
}
