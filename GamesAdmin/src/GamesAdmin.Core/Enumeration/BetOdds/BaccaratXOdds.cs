using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class BaccaratXOdds : Enumeration
    {
        public const string PlayerValue = "player";
        public const string BankValue = "banker";
        public const string TieValue = "tie";
        public const string PlayerPairValue = "playerPair";
        public const string BankPairValue = "bankerPair";

        public static readonly BaccaratXOdds Player = new BaccaratXOdds(PlayerValue, "Player");
        public static readonly BaccaratXOdds Bank = new BaccaratXOdds(BankValue, "Banker");
        public static readonly BaccaratXOdds PlayerPair = new BaccaratXOdds(PlayerPairValue, "Player Pair");
        public static readonly BaccaratXOdds BankerPair = new BaccaratXOdds(BankPairValue, "Banker Pair");

        public BaccaratXOdds() { }

        public BaccaratXOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
