using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Core.Models
{
    public class BetResult
    {
    }

    public class BigSmallBetResult : BetResult
    {
        public IEnumerable<byte> Dices { get; set; }

        public int Total => Dices.Sum(x => x);

        public BigSmallBetResult(byte firstDice, byte secondDice, byte thirdDice)
        {
            Dices = new List<byte>
            {
                firstDice, secondDice, thirdDice
            };
        }
    }

    public class OddEvenBetResult : BetResult
    {
        public IList<byte> Dices { get; set; }

        public int Total => Dices.Sum(x => x);

        public OddEvenBetResult(byte firstDice, byte secondDice, byte thirdDice)
        {
            Dices = new List<byte>
            {
                firstDice, secondDice, thirdDice
            };
        }
    }

    public class FishPrawnCrabBetResult : BetResult
    {
        public IList<byte> Dices { get; set; }

        public FishPrawnCrabBetResult(byte firstDice, byte secondDice, byte thirdDice)
        {
            Dices = new List<byte>
            {
                firstDice, secondDice, thirdDice
            };
        }
    }

    public class ShakeThePlateBetResult : BetResult
    {
        public IList<bool> RedChips { get; set; }

        public bool Even => NumberOfRedChips % 2 == 0;

        public bool Odd => !Even;

        public bool ThreeRed => NumberOfRedChips == 3;

        public bool FourRed => NumberOfRedChips == 4;

        public bool ThreeWhite => NumberOfRedChips == 1;

        public bool FourWhite => NumberOfRedChips == 0;

        private int NumberOfRedChips { get; }

        public ShakeThePlateBetResult(bool firstChip, bool secondChip, bool thirdChip, bool fourthChip)
        {
            RedChips = new List<bool>()
            {
                firstChip, secondChip, thirdChip, fourthChip
            };

            NumberOfRedChips = RedChips.Count(chip => chip);
        }
    }
}