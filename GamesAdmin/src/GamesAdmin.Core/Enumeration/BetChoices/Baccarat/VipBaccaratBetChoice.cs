namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipBaccaratBetChoice : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankValue = "banker";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string TieValue = "tie";
        public const string PlayerPairValue = "playerPair";
        public const string BankPairValue = "bankerPair";

        public static readonly VipBaccaratBetChoice Player = new VipBaccaratBetChoice(PlayerValue, "Player");
        public static readonly VipBaccaratBetChoice Bank = new VipBaccaratBetChoice(BankValue, "Banker");
        public static readonly VipBaccaratBetChoice Big = new VipBaccaratBetChoice(BigValue, "Big");
        public static readonly VipBaccaratBetChoice Small = new VipBaccaratBetChoice(SmallValue, "Small");
        public static readonly VipBaccaratBetChoice Tie = new VipBaccaratBetChoice(TieValue, "Tie");
        public static readonly VipBaccaratBetChoice PlayerPair = new VipBaccaratBetChoice(PlayerPairValue, "Player Pair");
        public static readonly VipBaccaratBetChoice BankerPair = new VipBaccaratBetChoice(BankPairValue, "Banker Pair");

        public VipBaccaratBetChoice() { }

        public VipBaccaratBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
