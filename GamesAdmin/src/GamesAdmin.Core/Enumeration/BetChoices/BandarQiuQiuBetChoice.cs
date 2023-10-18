namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BandarQiuQiuBetChoice : Enumeration
    {
        public const string SpadeValue = "spade";
        public const string HeartValue = "heart";
        public const string ClubValue = "club";
        public const string DiamondValue = "diamond";
        
        public static readonly BandarQiuQiuBetChoice Spade = new BandarQiuQiuBetChoice(SpadeValue, "Spade");
        public static readonly BandarQiuQiuBetChoice Heart = new BandarQiuQiuBetChoice(HeartValue, "Heart");
        public static readonly BandarQiuQiuBetChoice Club = new BandarQiuQiuBetChoice(ClubValue, "Club");
        public static readonly BandarQiuQiuBetChoice Diamond = new BandarQiuQiuBetChoice(DiamondValue, "Diamond");        

        public BandarQiuQiuBetChoice()
        { }

        public BandarQiuQiuBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
