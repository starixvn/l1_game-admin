namespace GamesAdmin.Core.Models
{
    public class BetChoice
    {
    }

    public class BigSmallChoice : BetChoice
    {
        public bool Big { get; set; }

        public decimal Amount { get; set; }

        public BigSmallChoice(bool isSelectBig, decimal amount)
        {
            Big = isSelectBig;
            Amount = amount;
        }
    }

    public class OddEvenChoice : BetChoice
    {
        public bool Even { get; set; }

        public decimal Amount { get; set; }

        public OddEvenChoice(bool isSelectEven, decimal amount)
        {
            Even = isSelectEven;
            Amount = amount;
        }
    }

    public class FishPrawnCrabChoice : BetChoice
    {
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }

        public decimal Amount => Stag + Gourd + Rooster + Fish + Crab + Prawn;

        public FishPrawnCrabChoice(decimal stag, decimal gourd, decimal rooster, decimal fish, decimal crab, decimal prawn)
        {
            Stag = stag;
            Gourd = gourd;
            Rooster = rooster;
            Fish = fish;
            Crab = crab;
            Prawn = prawn;
        }
    }

    public class ShakeThePlateChoice : BetChoice
    {
        public decimal Even { get; set; }

        public decimal Odd { get; set; }

        public decimal ThreeRed { get; set; }

        public decimal FourRed { get; set; }

        public decimal ThreeWhite { get; set; }

        public decimal FourWhite { get; set; }

        public decimal Amount => Even + Odd + ThreeRed + FourRed + ThreeWhite + FourWhite;

        public ShakeThePlateChoice(decimal even, decimal odd, decimal threeRed, decimal fourRed, decimal threeWhite, decimal fourWhite)
        {
            Even = even;
            Odd = odd;
            ThreeRed = threeRed;
            FourRed = fourRed;
            ThreeWhite = threeWhite;
            FourWhite = fourWhite;
        }
    }
}