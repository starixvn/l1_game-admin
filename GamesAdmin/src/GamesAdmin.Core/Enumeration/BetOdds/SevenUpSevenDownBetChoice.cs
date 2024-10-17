namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class SevenUpSevenDownOdds : Enumeration
    {
        public const string UpValue = "up";
        public const string DownValue = "down";
        public const string SevenValue = "seven";

        public static readonly SevenUpSevenDownOdds Up = new SevenUpSevenDownOdds(UpValue, "Up");
        public static readonly SevenUpSevenDownOdds Down = new SevenUpSevenDownOdds(DownValue, "Down");
        public static readonly SevenUpSevenDownOdds Seven = new SevenUpSevenDownOdds(SevenValue, "Seven");

        public SevenUpSevenDownOdds() { }

        public SevenUpSevenDownOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
