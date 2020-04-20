using System;

namespace GamesAdmin.Core.Models
{
    public abstract class Bet
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public decimal BetWin { get; set; }

        public IBetResult BetResult { get; set; }

        public IBetChoice Choice { get; set; }

        public Bet(
            long round,
            DateTime time,
            decimal betWin,
            IBetResult betResult,
            IBetChoice choice)
        {
            Round = round;
            Time = time;
            BetWin = betWin;
            BetResult = betResult;
            Choice = choice;
        }
    }

    public class BigSmallBet : Bet
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

    public class FishPrawnCrabBet : Bet
    {
        public FishPrawnCrabBet(
            long round,
            DateTime time,
            decimal betWin,
            FishPrawnCrabBetResult betResult,
            IBetChoice choice)
            : base(round, time, betWin, betResult, choice)
        {
        }
    }

    public class ShakeThePlateBet : Bet
    {
        public ShakeThePlateBet(
            long round,
            DateTime time,
            decimal betWin,
            ShakeThePlateBetResult betResult,
            IBetChoice choice)
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