namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class KenoPowerBetChoice : Enumeration
    {
        public const string BlueValue = "blue";
        public const string RedValue = "red";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string OddValue = "odd";
        public const string EvenValue = "even";
        public const string BlueBigValue = "blueBig";
        public const string BlueSmallValue = "blueSmall";
        public const string RedBigValue = "redBig";
        public const string RedSmallValue = "redSmall";

        public static readonly KenoPowerBetChoice Blue = new KenoPowerBetChoice(BlueValue, "Team Blue");
        public static readonly KenoPowerBetChoice Red = new KenoPowerBetChoice(RedValue, "Team Red");
        public static readonly KenoPowerBetChoice Big = new KenoPowerBetChoice(BigValue, "Sum Big");
        public static readonly KenoPowerBetChoice Small = new KenoPowerBetChoice(SmallValue, "Sum Small");
        public static readonly KenoPowerBetChoice Odd = new KenoPowerBetChoice(OddValue, "Sum Odd");
        public static readonly KenoPowerBetChoice Even = new KenoPowerBetChoice(EvenValue, "Sum even");
        public static readonly KenoPowerBetChoice BlueBig = new KenoPowerBetChoice(BlueBigValue, "Team Blue Big");
        public static readonly KenoPowerBetChoice BlueSmall = new KenoPowerBetChoice(BlueSmallValue, "Team Blue Small");
        public static readonly KenoPowerBetChoice RedBig = new KenoPowerBetChoice(RedBigValue, "Team Red Big");
        public static readonly KenoPowerBetChoice RedSmall = new KenoPowerBetChoice(RedSmallValue, "Team Red Small");


        public KenoPowerBetChoice() { }

        public KenoPowerBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
