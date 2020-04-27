using System;

namespace GamesAdmin.Core.Models
{
    public class BaseBetHistory 
    {
        public BaseBetHistory(long round, DateTime time,  decimal betWin)
        {
            Round = round;
            Time = time;
            BetWin = betWin;
        }

        public long Round { get; set; }

        public DateTime Time { get; set; }

        public decimal BetWin { get; set; }
    }

    public class BetHistory<TChoice, TResult> : BaseBetHistory where TChoice: BetChoice where TResult: BetResult
    {
        public TResult BetResult { get; set; }

        public TChoice Choice { get; set; }

        public BetHistory(
            long round,
            DateTime time,
            decimal betWin,
            TResult betResult,
            TChoice choice) : base(round, time,betWin)
        {
            BetResult = betResult;
            Choice = choice;
        }
    }

    public class BigSmallBet : BetHistory<BigSmallChoice, BigSmallBetResult>
    {
        public decimal Refund { get; set; }

        public BigSmallBet(
            long round,
            DateTime time,
            decimal betWin,
            BigSmallBetResult betResult,
            BigSmallChoice choice,
            decimal refund)
            : base(round, time, betWin, betResult, choice)
        {
            Refund = refund;
        }
    }

    public class OddEvenBet : BetHistory<OddEvenChoice, OddEvenBetResult>
    {
        public decimal Refund { get; set; }

        public OddEvenBet(
            long round,
            DateTime time,
            decimal betWin,
            OddEvenBetResult betResult,
            OddEvenChoice choice,
            decimal refund)
            : base(round, time, betWin, betResult, choice)
        {
            Refund = refund;
        }
    }

    public class FishPrawnCrabBet : BetHistory<FishPrawnCrabChoice, FishPrawnCrabBetResult>
    {
        public FishPrawnCrabBet(
            long round,
            DateTime time,
            decimal betWin,
            FishPrawnCrabBetResult betResult,
            FishPrawnCrabChoice choice)
            : base(round, time, betWin, betResult, choice)
        {
        }
    }

    public class ShakeThePlateBet : BetHistory<ShakeThePlateChoice, ShakeThePlateBetResult>
    {
        public ShakeThePlateBet(
            long round,
            DateTime time,
            decimal betWin,
            ShakeThePlateBetResult betResult,
            ShakeThePlateChoice choice)
            : base(round, time, betWin, betResult, choice)
        {
        }
    }

    public class BetInfo
    {
        public BetInfo(decimal bet, int round, bool select, string uid, bool sicbo)
        {
            Bet = bet;
            Round = round;
            Select = select;
            UID = uid;
            Sicbo = sicbo;
        }

        public decimal Bet { get; set; }

        public int Round { get; set; }

        public bool Select { get; set; }

        public string UID { get; set; }

        public bool Sicbo { get; set; }
    }
}