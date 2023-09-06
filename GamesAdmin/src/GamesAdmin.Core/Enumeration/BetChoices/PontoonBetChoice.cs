namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class PontoonBetChoice : Enumeration
    {
        public const string RegularValue = "regular";
        public const string PontoonValue = "pontoon";
        public const string FiveCardTrickValue = "fiveCardTrick";

        public static readonly PontoonBetChoice Regular = new PontoonBetChoice(RegularValue, "Regular");
        public static readonly PontoonBetChoice Pontoon = new PontoonBetChoice(PontoonValue, "Pontoon");
        public static readonly PontoonBetChoice FiveCard = new PontoonBetChoice(FiveCardTrickValue, "Five Card Trick");

        public PontoonBetChoice() { }

        public PontoonBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
