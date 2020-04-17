using Newtonsoft.Json;
using System.ComponentModel;

namespace GamesAdmin.Core.Models
{
    public interface IBetChoice
    {
    }

    public class BigSmallChoice : IBetChoice
    {
        public bool Big { get; set; }

        public decimal Amount { get; set; }

        public BigSmallChoice(bool isSelectBig, decimal amount)
        {
            Big = isSelectBig;
            Amount = amount;
        }
    }

    public class FishPrawnCrabChoice : IBetChoice
    {
        [JsonProperty(PropertyName = "1")]
        public decimal Stag { get; set; }

        [JsonProperty(PropertyName = "2")]
        public decimal Gourd { get; set; }

        [JsonProperty(PropertyName = "3")]
        public decimal Rooster { get; set; }

        [JsonProperty(PropertyName = "4")]
        public decimal Fish { get; set; }

        [JsonProperty(PropertyName = "5")]
        public decimal Crab { get; set; }

        [JsonProperty(PropertyName = "6")]
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

    public class ShakeThePlateChoice : IBetChoice
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