namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class DragonTigerBetChoice: Enumeration
    {
        public const string DragonValue = "dragon";
        public const string TigerValue = "tiger";
        public const string TieValue = "tie";

        public static readonly DragonTigerBetChoice Dragon = new DragonTigerBetChoice(DragonValue, "Dragon");
        public static readonly DragonTigerBetChoice Tiger = new DragonTigerBetChoice(TigerValue, "Tiger");
        public static readonly DragonTigerBetChoice Tie = new DragonTigerBetChoice(TieValue, "Tie");

        public DragonTigerBetChoice() { }

        public DragonTigerBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
