namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class HiLoBetOdds : Enumeration
    {
        public const string High3Value = "high3";
        public const string High4Value = "high4";
        public const string High5Value = "high5";
        public const string High6Value = "high6";
        public const string High7Value = "high7";
        public const string High8Value = "high8";
        public const string High9Value = "high9";
        public const string High10Value = "high10";
        public const string HighJValue = "high11";
        public const string HighQValue = "high12";
        public const string HighKValue = "high13";

        public const string Low3Value = "low3";
        public const string Low4Value = "low4";
        public const string Low5Value = "low5";
        public const string Low6Value = "low6";
        public const string Low7Value = "low7";
        public const string Low8Value = "low8";
        public const string Low9Value = "low9";
        public const string Low10Value = "low10";
        public const string LowJValue = "low11";
        public const string LowQValue = "low12";
        public const string LowKValue = "low13";


        public static readonly HiLoBetOdds High3 = new HiLoBetOdds(High3Value, "3 card");
        public static readonly HiLoBetOdds High4 = new HiLoBetOdds(High4Value, "4 card");
        public static readonly HiLoBetOdds High5 = new HiLoBetOdds(High5Value, "5 card");
        public static readonly HiLoBetOdds High6 = new HiLoBetOdds(High6Value, "6 card");
        public static readonly HiLoBetOdds High7 = new HiLoBetOdds(High7Value, "7 card");
        public static readonly HiLoBetOdds High8 = new HiLoBetOdds(High8Value, "8 card");
        public static readonly HiLoBetOdds High9 = new HiLoBetOdds(High9Value, "9 card");
        public static readonly HiLoBetOdds High10 = new HiLoBetOdds(High10Value, "10 card");
        public static readonly HiLoBetOdds HighJ = new HiLoBetOdds(HighJValue, "J card");
        public static readonly HiLoBetOdds HighQ = new HiLoBetOdds(HighQValue, "Q card");
        public static readonly HiLoBetOdds HighK = new HiLoBetOdds(HighKValue, "K card");

        public static readonly HiLoBetOdds Low3 = new HiLoBetOdds(Low3Value, "3 card");
        public static readonly HiLoBetOdds Low4 = new HiLoBetOdds(Low4Value, "4 card");
        public static readonly HiLoBetOdds Low5 = new HiLoBetOdds(Low5Value, "5 card");
        public static readonly HiLoBetOdds Low6 = new HiLoBetOdds(Low6Value, "6 card");
        public static readonly HiLoBetOdds Low7 = new HiLoBetOdds(Low7Value, "7 card");
        public static readonly HiLoBetOdds Low8 = new HiLoBetOdds(Low8Value, "8 card");
        public static readonly HiLoBetOdds Low9 = new HiLoBetOdds(Low9Value, "9 card");
        public static readonly HiLoBetOdds Low10 = new HiLoBetOdds(Low10Value, "10 card");
        public static readonly HiLoBetOdds LowJ = new HiLoBetOdds(LowJValue, "J card");
        public static readonly HiLoBetOdds LowQ = new HiLoBetOdds(LowQValue, "Q card");
        public static readonly HiLoBetOdds LowK = new HiLoBetOdds(LowKValue, "K card");


        public HiLoBetOdds() { }

        public HiLoBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
