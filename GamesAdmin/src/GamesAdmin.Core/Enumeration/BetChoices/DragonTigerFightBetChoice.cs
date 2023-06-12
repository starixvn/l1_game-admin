namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class DragonTigerFightBetChoice : Enumeration
    {
        public const string DragonValue = "dragon";
        public const string TigerValue = "tiger";
        public const string TieValue = "tie";
        public const string DragonRedValue = "dragonRed";
        public const string DragonBlackValue = "dragonBlack";
        public const string TigerRedValue = "tigerRed";
        public const string TigerBlackValue = "tigerBlack";

        public static readonly DragonTigerFightBetChoice Dragon = new DragonTigerFightBetChoice(DragonValue, "Dragon");
        public static readonly DragonTigerFightBetChoice Tiger = new DragonTigerFightBetChoice(TigerValue, "Tiger");
        public static readonly DragonTigerFightBetChoice Tie = new DragonTigerFightBetChoice(TieValue, "Tie");
        public static readonly DragonTigerFightBetChoice DragonRed = new DragonTigerFightBetChoice(DragonRedValue, "Dragon Red");
        public static readonly DragonTigerFightBetChoice DragonBlack = new DragonTigerFightBetChoice(DragonBlackValue, "Dragon Black");
        public static readonly DragonTigerFightBetChoice TigerRed = new DragonTigerFightBetChoice(TigerRedValue, "Tiger Red");
        public static readonly DragonTigerFightBetChoice TigerBlack = new DragonTigerFightBetChoice(TigerBlackValue, "Tiger Black");

        public DragonTigerFightBetChoice()
        { }

        public DragonTigerFightBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}