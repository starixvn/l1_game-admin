namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class KenoPowerBetChoice : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankValue = "banker";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string OddValue = "odd";
        public const string EvenValue = "even";
        public const string RedValue = "red";
        public const string OrangeValue = "orange";
        public const string YellowValue = "yellow";
        public const string GreenValue = "green";
        public const string BlueValue = "blue";
        public const string IndigoValue = "indigo";
        public const string VioletValue = "viloet";
        public const string PlayerBigValue = "playerBig";
        public const string PlayerSmallValue = "playerSmall";
        public const string BankerBigValue = "bankerBig";
        public const string BankerSmallValue = "bankerSmall";

        public static readonly KenoPowerBetChoice Player = new KenoPowerBetChoice(PlayerValue, "Player");
        public static readonly KenoPowerBetChoice Bank = new KenoPowerBetChoice(BankValue, "Banker");
        public static readonly KenoPowerBetChoice Big = new KenoPowerBetChoice(BigValue, "Sum Big");
        public static readonly KenoPowerBetChoice Small = new KenoPowerBetChoice(SmallValue, "Sum Small");
        public static readonly KenoPowerBetChoice Odd = new KenoPowerBetChoice(OddValue, "Sum Odd");
        public static readonly KenoPowerBetChoice Even = new KenoPowerBetChoice(EvenValue, "Sum even");
        public static readonly KenoPowerBetChoice Red = new KenoPowerBetChoice(RedValue, "Red");
        public static readonly KenoPowerBetChoice Orange = new KenoPowerBetChoice(OrangeValue, "Orange");
        public static readonly KenoPowerBetChoice Yellow = new KenoPowerBetChoice(YellowValue, "Yellow");
        public static readonly KenoPowerBetChoice Green = new KenoPowerBetChoice(GreenValue, "Green");
        public static readonly KenoPowerBetChoice Blue = new KenoPowerBetChoice(BlueValue, "Blue");
        public static readonly KenoPowerBetChoice Indigo = new KenoPowerBetChoice(IndigoValue, "Indigo");
        public static readonly KenoPowerBetChoice Violet = new KenoPowerBetChoice(VioletValue, "Violet");
        public static readonly KenoPowerBetChoice PlayerBig = new KenoPowerBetChoice(PlayerBigValue, "Player Big");
        public static readonly KenoPowerBetChoice PlayerSmall = new KenoPowerBetChoice(PlayerSmallValue, "Player Small");
        public static readonly KenoPowerBetChoice BankerBig = new KenoPowerBetChoice(BankerBigValue, "Banker Big");
        public static readonly KenoPowerBetChoice BankerSmall = new KenoPowerBetChoice(BankerSmallValue, "Banker Small");


        public KenoPowerBetChoice() { }

        public KenoPowerBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
