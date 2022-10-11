namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FanTanBetChoice : Enumeration
    {        
        public const string EvenValue = "even";
        public const string OddValue = "odd";

        public const string Fan1Value = "fan1";
        public const string Fan2Value = "fan2";
        public const string Fan3Value = "fan3";
        public const string Fan4Value = "fan4";

        public const string Kwok12Value = "kwok12";
        public const string Kwok23Value = "kwok23";
        public const string Kwok34Value = "kwok34";
        public const string Kwok41Value = "kwok41";

        public const string Chun123Value = "chun123";
        public const string Chun124Value = "chun124";
        public const string Chun134Value = "chun134";
        public const string Chun234Value = "chun234";

        public static readonly FanTanBetChoice Even = new FanTanBetChoice(EvenValue, "Even");
        public static readonly FanTanBetChoice Odd = new FanTanBetChoice(OddValue, "Odd");

        public static readonly FanTanBetChoice Fan1 = new FanTanBetChoice(Fan1Value, "1 Fan");
        public static readonly FanTanBetChoice Fan2 = new FanTanBetChoice(Fan2Value, "2 Fan");
        public static readonly FanTanBetChoice Fan3 = new FanTanBetChoice(Fan3Value, "3 Fan");
        public static readonly FanTanBetChoice Fan4 = new FanTanBetChoice(Fan4Value, "4 Fan");

        public static readonly FanTanBetChoice Kwok12 = new FanTanBetChoice(Kwok12Value, "1 2 Kwok");
        public static readonly FanTanBetChoice Kwok23 = new FanTanBetChoice(Kwok23Value, "2 3 Kwok");
        public static readonly FanTanBetChoice Kwok34 = new FanTanBetChoice(Kwok34Value, "3 4 Kwok");
        public static readonly FanTanBetChoice Kwok41 = new FanTanBetChoice(Kwok41Value, "4 1 Kwok");

        public static readonly FanTanBetChoice Chun123 = new FanTanBetChoice(Chun123Value, "1 2 3 Chun");
        public static readonly FanTanBetChoice Chun124 = new FanTanBetChoice(Chun124Value, "1 2 4 Chun");
        public static readonly FanTanBetChoice Chun134 = new FanTanBetChoice(Chun134Value, "1 3 4 Chun");
        public static readonly FanTanBetChoice Chun234 = new FanTanBetChoice(Chun234Value, "2 3 4 Chun");

        public FanTanBetChoice() { }

        public FanTanBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
