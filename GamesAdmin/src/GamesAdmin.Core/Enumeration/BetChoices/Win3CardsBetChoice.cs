namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class Win3CardsBetChoice : Enumeration
    {
        public const string DragonValue = "dragon";
        public const string PhoenixValue = "phoenix";
        public const string Pair8PlusValue = "pair8Plus";
        public const string StraightValue = "straight";
        public const string FlushValue = "flush";
        public const string StraightFlushValue = "straightFlush";
        public const string ThreeKindValue = "threeOfAKind";

        public static readonly Win3CardsBetChoice Dragon = new Win3CardsBetChoice(DragonValue, "Dragon");
        public static readonly Win3CardsBetChoice Phoenix = new Win3CardsBetChoice(PhoenixValue, "Phoenix");
        public static readonly Win3CardsBetChoice Pair8Plus = new Win3CardsBetChoice(Pair8PlusValue, "Pair 8 Plus");
        public static readonly Win3CardsBetChoice Straight = new Win3CardsBetChoice(StraightValue, "Straight");
        public static readonly Win3CardsBetChoice Flush = new Win3CardsBetChoice(FlushValue, "Flush");
        public static readonly Win3CardsBetChoice StraightFlush = new Win3CardsBetChoice(StraightFlushValue, "Straight Flush");
        public static readonly Win3CardsBetChoice ThreeKind = new Win3CardsBetChoice(ThreeKindValue, "Three of a Kind");

        public Win3CardsBetChoice() { }

        public Win3CardsBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
