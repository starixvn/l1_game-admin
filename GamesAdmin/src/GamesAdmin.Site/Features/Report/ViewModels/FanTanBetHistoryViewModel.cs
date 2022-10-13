using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class FanTanBetHistoryViewModel : BetHistoryViewModel
    {
        public FanTanRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<FanTanBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class FanTanBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public Dictionary<string, decimal> Odds { get; set; }
        public decimal BetWin { get; set; }

        public decimal Odd { get; set; }
        public decimal Even { get; set; }

        public decimal Fan1 { get; set; }
        public decimal Fan2 { get; set; }
        public decimal Fan3 { get; set; }
        public decimal Fan4 { get; set; }

        public decimal Kwok12 { get; set; }
        public decimal Kwok23 { get; set; }
        public decimal Kwok34 { get; set; }
        public decimal Kwok41 { get; set; }

        public decimal Chun123 { get; set; }
        public decimal Chun124 { get; set; }
        public decimal Chun134 { get; set; }
        public decimal Chun234 { get; set; }

        public string GetBetChoice()
        {
            if (Odd > 0)
            {
                return "Odd";
            }

            if (Even > 0)
            {
                return "Even";
            }

            if (Fan1 > 0)
            {
                return "Fan1";
            }

            if (Fan2 > 0)
            {
                return "Fan2";
            }

            if (Fan3 > 0)
            {
                return "Fan3";
            }

            if (Fan4 > 0)
            {
                return "Fan4";
            }

            if (Kwok12 > 0)
            {
                return "Kwok12";
            }

            if (Kwok23 > 0)
            {
                return "Kwok23";
            }

            if (Kwok34 > 0)
            {
                return "Kwok34";
            }

            if (Kwok41 > 0)
            {
                return "Kwok41";
            }

            if (Chun123 > 0)
            {
                return "Chun123";
            }

            if (Chun124 > 0)
            {
                return "Chun124";
            }

            if (Chun134 > 0)
            {
                return "Chun134";
            }

            if (Chun234 > 0)
            {
                return "Chun234";
            }

            return string.Empty;
        }

        public decimal GetBetAmount()
            => Odd + Even + Fan1 + Fan2 + Fan3 + Fan4 + Kwok12 + Kwok23 + Kwok34 + Kwok41 + Chun123 + Chun124 + Chun134 + Chun234;
    }
}