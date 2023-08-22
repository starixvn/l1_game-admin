namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class ColorGameBetChoice : Enumeration
    {
        public const string YellowValue = "yellow";
        public const string RedValue = "red";
        public const string PinkValue = "pink";
        public const string WhiteValue = "white";
        public const string BlueValue = "blue";
        public const string GreenValue = "green";
        public const string HotValue = "hot";
        public const string ColdValue = "cold";

        public static readonly ColorGameBetChoice Yellow = new ColorGameBetChoice(YellowValue, "Yellow");
        public static readonly ColorGameBetChoice Red = new ColorGameBetChoice(RedValue, "Red");
        public static readonly ColorGameBetChoice Pink = new ColorGameBetChoice(PinkValue, "Pink");
        public static readonly ColorGameBetChoice White = new ColorGameBetChoice(WhiteValue, "White");
        public static readonly ColorGameBetChoice Blue = new ColorGameBetChoice(BlueValue, "Blue");
        public static readonly ColorGameBetChoice Green = new ColorGameBetChoice(GreenValue, "Green");
        public static readonly ColorGameBetChoice Hot = new ColorGameBetChoice(HotValue, "Hot");
        public static readonly ColorGameBetChoice Cold = new ColorGameBetChoice(ColdValue, "Cold");

        public ColorGameBetChoice()
        { }

        public ColorGameBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
