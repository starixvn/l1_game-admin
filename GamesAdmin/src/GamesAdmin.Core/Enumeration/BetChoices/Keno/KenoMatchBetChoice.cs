namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class KenoMatchBetChoice : Enumeration
    {
        public const string FHBlueValue = "fhBlue";
        public const string FHRedValue = "fhRed";
        public const string FHOverValue = "fhOver";
        public const string FHUnderValue = "fhUnder";
        public const string FHOddValue = "fhOdd";
        public const string FHEvenValue = "fhEven";

        public const string FTBlueValue = "ftBlue";
        public const string FTRedValue = "ftRed";
        public const string FTOverValue = "ftOver";
        public const string FTUnderValue = "ftUnder";
        public const string FTOddValue = "ftOdd";
        public const string FTEvenValue = "ftEven";

        public const string LiveFTRedValue = "liveFTRed";
        public const string LiveFTBlueValue = "liveFTBlue";
        public const string LiveFTOddValue = "liveFTOdd";
        public const string LiveFTEvenValue = "liveFTEven";
        public const string LiveFTOverValue = "liveFTOver";
        public const string LiveFTUnderValue = "liveFTUnder";

        public static readonly KenoMatchBetChoice FHBlue = new KenoMatchBetChoice(FHBlueValue, "First Half Team Blue");
        public static readonly KenoMatchBetChoice FHRed = new KenoMatchBetChoice(FHRedValue, "First Half Team Red");
        public static readonly KenoMatchBetChoice FHOver = new KenoMatchBetChoice(FHOverValue, "First Half Over");
        public static readonly KenoMatchBetChoice FHUnder = new KenoMatchBetChoice(FHUnderValue, "First Half Under");
        public static readonly KenoMatchBetChoice FHOdd = new KenoMatchBetChoice(FHOddValue, "First Half Odd");
        public static readonly KenoMatchBetChoice FHEven = new KenoMatchBetChoice(FHEvenValue, "First Half Even");

        public static readonly KenoMatchBetChoice FTBlue = new KenoMatchBetChoice(FTBlueValue, "Full Time Team Blue");
        public static readonly KenoMatchBetChoice FTRed = new KenoMatchBetChoice(FTRedValue, "Full Time Team Red");
        public static readonly KenoMatchBetChoice FTOver = new KenoMatchBetChoice(FTOverValue, "Full Time Over");
        public static readonly KenoMatchBetChoice FTUnder = new KenoMatchBetChoice(FTUnderValue, "Full Time Under");
        public static readonly KenoMatchBetChoice FTOdd = new KenoMatchBetChoice(FTOddValue, "Full Time Odd");
        public static readonly KenoMatchBetChoice FTEven = new KenoMatchBetChoice(FTEvenValue, "Full Time Even");

        public static readonly KenoMatchBetChoice LiveFTOdd = new KenoMatchBetChoice(LiveFTOddValue, "Full Time (Live) Odd");
        public static readonly KenoMatchBetChoice LiveFTEven = new KenoMatchBetChoice(LiveFTEvenValue, "Full Time (Live) Even");

        public KenoMatchBetChoice()
        {
        }

        public KenoMatchBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}