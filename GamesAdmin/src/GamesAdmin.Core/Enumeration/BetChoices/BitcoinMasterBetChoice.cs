namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BitcoinMasterBetChoice : Enumeration
    {
        public const string UpValue = "up";
        public const string DownValue = "down";
        public const string OddValue = "odd";
        public const string EvenValue = "even";

        public static readonly BitcoinMasterBetChoice Up = new BitcoinMasterBetChoice(UpValue, "Up");
        public static readonly BitcoinMasterBetChoice Down = new BitcoinMasterBetChoice(DownValue, "Down");
        public static readonly BitcoinMasterBetChoice Odd = new BitcoinMasterBetChoice(OddValue, "Odd");
        public static readonly BitcoinMasterBetChoice Even = new BitcoinMasterBetChoice(EvenValue, "Even");

        public BitcoinMasterBetChoice() { }

        public BitcoinMasterBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
