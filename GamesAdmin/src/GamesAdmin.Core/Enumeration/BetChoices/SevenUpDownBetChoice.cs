namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class SevenUpDownBetChoice : Enumeration
    {
        public const string UpValue = "up";
        public const string DownValue = "down";
        public const string SevenValue = "seven";

        public static readonly SevenUpDownBetChoice Up = new SevenUpDownBetChoice(UpValue, "Up");
        public static readonly SevenUpDownBetChoice Down = new SevenUpDownBetChoice(DownValue, "Down");
        public static readonly SevenUpDownBetChoice Seven = new SevenUpDownBetChoice(SevenValue, "Seven");

        public SevenUpDownBetChoice() { }

        public SevenUpDownBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
