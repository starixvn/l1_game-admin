namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class HiLoBetChoice : Enumeration
    {
        public const string HighValue = "h";
        public const string LowValue = "l";

        public static readonly HiLoBetChoice High = new HiLoBetChoice(HighValue, "High");
        public static readonly HiLoBetChoice Low = new HiLoBetChoice(LowValue, "Low");

        public HiLoBetChoice() { }

        public HiLoBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
