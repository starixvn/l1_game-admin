namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class ChineseBlackjackOdds : Enumeration
    {
        public const string RegularValue = "regular";
        public const string BanbanValue = "banBan";
        public const string BanluckValue = "banLuck";
        public const string FiveCardTrickValue = "fiveCardTrick";

        public static readonly ChineseBlackjackOdds Regular = new ChineseBlackjackOdds(RegularValue, "Regular");
        public static readonly ChineseBlackjackOdds Banban = new ChineseBlackjackOdds(BanbanValue, "Ban-Ban");
        public static readonly ChineseBlackjackOdds Banluck = new ChineseBlackjackOdds(BanluckValue, "Ban-Luck");
        public static readonly ChineseBlackjackOdds FiveCard = new ChineseBlackjackOdds(FiveCardTrickValue, "Five Card Trick");

        public ChineseBlackjackOdds() { }

        public ChineseBlackjackOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
