using GamesAdmin.Core.Enumeration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Core.Models
{
    public interface IBetResult
    {
    }

    public class BigSmallBetResult : IBetResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public int Total => FirstDice + SecondDice + ThirdDice;

        public BigSmallBetResult(byte firstDice, byte secondDice, byte thirdDice)
        {
            FirstDice = firstDice;
            SecondDice = secondDice;
            ThirdDice = thirdDice;
        }
    }

    public class FishPrawnCrabBetResult : IBetResult
    {
        public byte FirstDice { get; set; }

        public byte SecondDice { get; set; }

        public byte ThirdDice { get; set; }

        public FishPrawnCrabBetResult(byte firstDice, byte secondDice, byte thirdDice)
        {
            FirstDice = firstDice;
            SecondDice = secondDice;
            ThirdDice = thirdDice;
        }
    }

    public class ShakeThePlateBetResult : IBetResult
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