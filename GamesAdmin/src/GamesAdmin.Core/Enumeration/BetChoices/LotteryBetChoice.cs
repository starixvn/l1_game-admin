namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class LotteryBetChoice : Enumeration
    {
        public LotteryBetChoice()
        {
        }

        public LotteryBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }

        public const string FourSetsValue = "4Sets";
        public const string SpecialValue = "special";
        public const string HeadValue = "head";
        public const string TailValue = "tail";
        public const string TwentySevenSetsValue = "27Sets";
        public const string Parlay2Value = "parlay2";
        public const string Parlay3Value = "parlay3";
        public const string Parlay4Value = "parlay4";
        public const string OthersValue = "others";

        public static readonly LotteryBetChoice FourSets = new LotteryBetChoice(FourSetsValue, "4 Sets");
        public static readonly LotteryBetChoice Special = new LotteryBetChoice(SpecialValue, "Special");
        public static readonly LotteryBetChoice Head = new LotteryBetChoice(HeadValue, "Head");
        public static readonly LotteryBetChoice Tail = new LotteryBetChoice(TailValue, "Tail");
        public static readonly LotteryBetChoice TwentySevenSets = new LotteryBetChoice(TwentySevenSetsValue, "27 Sets");
        public static readonly LotteryBetChoice Parlay2 = new LotteryBetChoice(Parlay2Value, "Parlay 2");
        public static readonly LotteryBetChoice Parlay3 = new LotteryBetChoice(Parlay3Value, "Parlay 3");
        public static readonly LotteryBetChoice Parlay4 = new LotteryBetChoice(Parlay4Value, "Parlay 4");
        public static readonly LotteryBetChoice Others = new LotteryBetChoice(OthersValue, "Others");

    }
}