namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class SevenUpSevenDownBetChoice : Enumeration
    {
        public const string UpValue = "up";
        public const string DownValue = "down";
        public const string SevenValue = "seven";

        public static readonly SevenUpSevenDownBetChoice Up = new SevenUpSevenDownBetChoice(UpValue, "Up");
        public static readonly SevenUpSevenDownBetChoice Down = new SevenUpSevenDownBetChoice(DownValue, "Down");
        public static readonly SevenUpSevenDownBetChoice Seven = new SevenUpSevenDownBetChoice(SevenValue, "Seven");

        public SevenUpSevenDownBetChoice() { }

        public SevenUpSevenDownBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
