namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class BandarQiuQiuOdds : Enumeration
    {
        public const string SixDevilValue = "sixDevil";
        public const string TwinValue = "twin";
        public const string SmallValue = "small";
        public const string BigValue = "big";
        public const string QiuQiuValue = "qiuQiu";
        public const string OrdinaryCardValue = "ordinaryCard";

        public static readonly BandarQiuQiuOdds SixDevil = new BandarQiuQiuOdds(SixDevilValue, "Six Devil");
        public static readonly BandarQiuQiuOdds Twin = new BandarQiuQiuOdds(TwinValue, "Twin");
        public static readonly BandarQiuQiuOdds Small = new BandarQiuQiuOdds(SmallValue, "Small");
        public static readonly BandarQiuQiuOdds Big = new BandarQiuQiuOdds(BigValue, "Big");
        public static readonly BandarQiuQiuOdds QiuQiu = new BandarQiuQiuOdds(QiuQiuValue, "Qiu Qiu");
        public static readonly BandarQiuQiuOdds OrdinaryCard = new BandarQiuQiuOdds(OrdinaryCardValue, "Ordinary Card");

        public BandarQiuQiuOdds() { }

        public BandarQiuQiuOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
