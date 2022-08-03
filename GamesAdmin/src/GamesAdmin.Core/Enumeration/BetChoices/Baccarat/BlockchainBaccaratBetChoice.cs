namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BlockchainBaccaratBetChoice : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankValue = "banker";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string TieValue = "tie";
        public const string PlayerPairValue = "playerPair";
        public const string BankPairValue = "bankerPair";

        public static readonly BlockchainBaccaratBetChoice Player = new BlockchainBaccaratBetChoice(PlayerValue, "Player");
        public static readonly BlockchainBaccaratBetChoice Bank = new BlockchainBaccaratBetChoice(BankValue, "Banker");
        public static readonly BlockchainBaccaratBetChoice Big = new BlockchainBaccaratBetChoice(BigValue, "Big");
        public static readonly BlockchainBaccaratBetChoice Small = new BlockchainBaccaratBetChoice(SmallValue, "Small");
        public static readonly BlockchainBaccaratBetChoice Tie = new BlockchainBaccaratBetChoice(TieValue, "Tie");
        public static readonly BlockchainBaccaratBetChoice PlayerPair = new BlockchainBaccaratBetChoice(PlayerPairValue, "Player Pair");
        public static readonly BlockchainBaccaratBetChoice BankerPair = new BlockchainBaccaratBetChoice(BankPairValue, "Banker Pair");

        public BlockchainBaccaratBetChoice() { }

        public BlockchainBaccaratBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
