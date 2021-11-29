namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class KenoMatchBetOdds : Enumeration
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
        public const string LiveFTOddValue = "liveFTOdd";
        public const string LiveFTEvenValue = "liveFTEven";

        public static readonly KenoMatchBetOdds FHBlue = new KenoMatchBetOdds(FHBlueValue, "First Half Team Blue");
        public static readonly KenoMatchBetOdds FHRed = new KenoMatchBetOdds(FHRedValue, "First Half Team Red");
        public static readonly KenoMatchBetOdds FHOver = new KenoMatchBetOdds(FHOverValue, "First Half Over");
        public static readonly KenoMatchBetOdds FHUnder = new KenoMatchBetOdds(FHUnderValue, "First Half Under");
        public static readonly KenoMatchBetOdds FHOdd = new KenoMatchBetOdds(FHOddValue, "First Half Odd");
        public static readonly KenoMatchBetOdds FHEven = new KenoMatchBetOdds(FHEvenValue, "First Half Even");

        public static readonly KenoMatchBetOdds FTBlue = new KenoMatchBetOdds(FTBlueValue, "Full Time Team Blue");
        public static readonly KenoMatchBetOdds FTRed = new KenoMatchBetOdds(FTRedValue, "Full Time Team Red");
        public static readonly KenoMatchBetOdds FTOver = new KenoMatchBetOdds(FTOverValue, "Full Time Over");
        public static readonly KenoMatchBetOdds FTUnder = new KenoMatchBetOdds(FTUnderValue, "Full Time Under");
        public static readonly KenoMatchBetOdds FTOdd = new KenoMatchBetOdds(FTOddValue, "Full Time Odd");
        public static readonly KenoMatchBetOdds FTEven = new KenoMatchBetOdds(FTEvenValue, "Full Time Even");

        public static readonly KenoMatchBetOdds LiveFTOdd = new KenoMatchBetOdds(LiveFTOddValue, "Full Time (Live) Odd");
        public static readonly KenoMatchBetOdds LiveFTEven = new KenoMatchBetOdds(LiveFTEvenValue, "Full Time (Live) Even");

        public KenoMatchBetOdds()
        {
        }

        public KenoMatchBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}