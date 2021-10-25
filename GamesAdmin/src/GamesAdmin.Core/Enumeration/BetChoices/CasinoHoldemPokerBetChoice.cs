namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class CasinoHoldemPokerBetChoice : Enumeration
    {
        public const string Ante = "ante";
        public const string AA = "aa";
        public const string Call = "call";

        public static readonly CasinoHoldemPokerBetChoice Even = new CasinoHoldemPokerBetChoice(Ante, "Ante");
        public static readonly CasinoHoldemPokerBetChoice Odd = new CasinoHoldemPokerBetChoice(AA, "AA");
        public static readonly CasinoHoldemPokerBetChoice Red3 = new CasinoHoldemPokerBetChoice(Call, "Call");

        public CasinoHoldemPokerBetChoice() { }

        public CasinoHoldemPokerBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
