namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BaccaratBetChoice : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankValue = "banker";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string TieValue = "tie";
        public const string PlayerPairValue = "playerpair";
        public const string BankPairValue = "bankerpair";

        public static readonly BaccaratBetChoice Player = new BaccaratBetChoice(PlayerValue, "Player");
        public static readonly BaccaratBetChoice Bank = new BaccaratBetChoice(BankValue, "Banker");
        public static readonly BaccaratBetChoice Big = new BaccaratBetChoice(BigValue, "Big");
        public static readonly BaccaratBetChoice Small = new BaccaratBetChoice(SmallValue, "Small");
        public static readonly BaccaratBetChoice Tie = new BaccaratBetChoice(TieValue, "Tie");
        public static readonly BaccaratBetChoice PlayerPair = new BaccaratBetChoice(PlayerPairValue, "Player Pair");
        public static readonly BaccaratBetChoice BankerPair = new BaccaratBetChoice(BankPairValue, "Banker Pair");

        public BaccaratBetChoice() { }

        public BaccaratBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
