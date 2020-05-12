namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class OddEvenBetChoice : Enumeration
    {        
        public const string EvenValue = "even";
        public const string OddValue = "odd";
       
        public static readonly OddEvenBetChoice Even = new OddEvenBetChoice(EvenValue, "Even");
        public static readonly OddEvenBetChoice Odd = new OddEvenBetChoice(OddValue, "Odd");

        public OddEvenBetChoice() { }

        public OddEvenBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
