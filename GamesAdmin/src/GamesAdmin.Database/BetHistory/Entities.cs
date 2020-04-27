using System;
using System.Collections.Generic;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Database
{
    public abstract class BetHistoryReportEntity
    {
        public DateTime Time { get; set; }

        public string Name { get; set; }

        public long Round { get; set; }

        public decimal BetWin { get; set; }
    }

    public class BigSmallBetHistoryReportEntity : BetHistoryReportEntity
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public decimal Bet { get; set; }

        public bool Select { get; set; }

        public decimal Refund { get; set; }
    }

    public class OddEvenBetHistoryReportEntity : BetHistoryReportEntity
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public decimal Bet { get; set; }

        public bool Select { get; set; }

        public decimal Refund { get; set; }
    }

    public class FishPrawnCrabReportEntity : BetHistoryReportEntity
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }
    }

    public class ShakeThePlateReportEntity : BetHistoryReportEntity
    {
        public bool FirstRedChip { get; set; }

        public bool SecondRedChip { get; set; }

        public bool ThirdRedChip { get; set; }

        public bool FourthRedChip { get; set; }

        public decimal Even { get; set; }

        public decimal Odd { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }
    }

    internal class FishPrawnCrabRoundWithBetsEntity : FishPrawnCrabRoundEntity
    {
        public IEnumerable<FishPrawnCrabBetEntity> Bet { get; set; }
    }

    internal class FishPrawnCrabRoundWithBetUnwindEntity : FishPrawnCrabRoundEntity
    {
        public FishPrawnCrabBetEntity Bet { get; set; }

        public BetHistoryReportEntity ToGameReport()
        {
            return new FishPrawnCrabReportEntity
            {
                Name = Bet.Name,
                BetWin = Bet.Betwin,
                Round = Number,
                Time = Time,
                FirstDice = (byte)(FirstDice + 1),
                SecondDice = (byte)(SecondDice + 1),
                ThirdDice = (byte)(ThirdDice + 1),
                Stag = Bet.Stag,
                Gourd = Bet.Gourd,
                Crab = Bet.Crab,
                Fish = Bet.Fish,
                Prawn = Bet.Prawn,
                Rooster = Bet.Rooster
            };
        }
    }

    internal class ShakeThePlateRoundWithBetsEntity : ShakeThePlateRoundEntity
    {
        public IEnumerable<ShakeThePlateBetEntity> Bet { get; set; }
    }

    internal class ShakeThePlateRoundWithBetUnwindEntity : ShakeThePlateRoundEntity
    {
        public ShakeThePlateBetEntity Bet { get; set; }

        public BetHistoryReportEntity ToGameReport()
        {
            return new ShakeThePlateReportEntity
            {
                Name = Bet.Name,
                BetWin = Bet.Betwin,
                Round = Number,
                Time = Time,
                Even = Bet.Even,
                Odd = Bet.Odd,
                ThreeRed = Bet.ThreeRed,
                FourRed = Bet.FourRed,
                ThreeWhite = Bet.ThreeWhite,
                FourWhite = Bet.FourWhite,
                FirstRedChip = FirstRedChip,
                SecondRedChip = SecondRedChip,
                ThirdRedChip = ThirdRedChip,
                FourthRedChip = FourthRedChip
            };
        }
    }

    internal class BigSmallRoundWithOnesEntity : BigSmallRoundEntity
    {
        public IEnumerable<BigSmallOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class BigSmallRoundWithOneUnwindEntity : BigSmallRoundEntity
    {
        public BigSmallOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class BigSmallTurboRoundWithOnesEntity : BigSmallTurboRoundEntity
    {
        public IEnumerable<BigSmallTurboOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class BigSmallTurboRoundWithOneUnwindEntity : BigSmallTurboRoundEntity
    {
        public BigSmallTurboOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class OddEvenRoundWithOnesEntity : OddEvenRoundEntity
    {
        public IEnumerable<OddEvenOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class OddEvenRoundWithOneUnwindEntity : OddEvenRoundEntity
    {
        public OddEvenOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class OddEvenTurboRoundWithOnesEntity : OddEvenTurboRoundEntity
    {
        public IEnumerable<OddEvenTurboOnesEntity> One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }

    internal class OddEvenTurboRoundWithOneUnwindEntity : OddEvenTurboRoundEntity
    {
        public OddEvenTurboOnesEntity One { get; set; }

        public IEnumerable<AccountInfoEntity> Accounts { get; set; }
    }
}