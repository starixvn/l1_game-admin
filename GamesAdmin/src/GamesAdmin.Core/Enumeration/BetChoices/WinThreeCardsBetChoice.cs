namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class WinThreeCardsBetChoice : Enumeration
    {
        public const string DragonValue = "dragon";
        public const string PhoenixValue = "phoenix";
        public const string Pair8PlusValue = "pair8Plus";
        public const string StraightValue = "straight";
        public const string FlushValue = "flush";
        public const string StraightFlushValue = "straightFlush";
        public const string ThreeKindValue = "threeOfAKind";

        public static readonly WinThreeCardsBetChoice Dragon = new WinThreeCardsBetChoice(DragonValue, "Dragon");
        public static readonly WinThreeCardsBetChoice Phoenix = new WinThreeCardsBetChoice(PhoenixValue, "Phoenix");
        public static readonly WinThreeCardsBetChoice Pair8Plus = new WinThreeCardsBetChoice(Pair8PlusValue, "Pair 8 Plus");
        public static readonly WinThreeCardsBetChoice Straight = new WinThreeCardsBetChoice(StraightValue, "Straight");
        public static readonly WinThreeCardsBetChoice Flush = new WinThreeCardsBetChoice(FlushValue, "Flush");
        public static readonly WinThreeCardsBetChoice StraightFlush = new WinThreeCardsBetChoice(StraightFlushValue, "Straight Flush");
        public static readonly WinThreeCardsBetChoice ThreeKind = new WinThreeCardsBetChoice(ThreeKindValue, "Three of a Kind");

        public WinThreeCardsBetChoice() { }

        public WinThreeCardsBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
