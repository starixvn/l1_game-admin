namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class SevenUpDownOdds : Enumeration
    {
        public const string UpValue = "up";
        public const string DownValue = "down";
        public const string SevenValue = "seven";

        public static readonly SevenUpDownOdds Up = new SevenUpDownOdds(UpValue, "Up");
        public static readonly SevenUpDownOdds Down = new SevenUpDownOdds(DownValue, "Down");
        public static readonly SevenUpDownOdds Seven = new SevenUpDownOdds(SevenValue, "Seven");

        public SevenUpDownOdds() { }

        public SevenUpDownOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
