namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BigSmallBetChoice : Enumeration
    {
        public const string BigValue = "big";
        public const string SmallValue = "small";

        public static readonly BigSmallBetChoice Big = new BigSmallBetChoice(BigValue, "Big");
        public static readonly BigSmallBetChoice Small = new BigSmallBetChoice(SmallValue, "Small");

        public BigSmallBetChoice() { }

        public BigSmallBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
