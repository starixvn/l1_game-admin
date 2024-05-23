namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BaccaratXBetChoice : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankValue = "banker";
        public const string TieValue = "tie";
        public const string PlayerPairValue = "playerPair";
        public const string BankPairValue = "bankerPair";

        public static readonly BaccaratXBetChoice Player = new BaccaratXBetChoice(PlayerValue, "Player");
        public static readonly BaccaratXBetChoice Bank = new BaccaratXBetChoice(BankValue, "Banker");
        public static readonly BaccaratXBetChoice PlayerPair = new BaccaratXBetChoice(PlayerPairValue, "Player Pair");
        public static readonly BaccaratXBetChoice BankerPair = new BaccaratXBetChoice(BankPairValue, "Banker Pair");

        public BaccaratXBetChoice() { }

        public BaccaratXBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
