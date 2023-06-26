namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class DragonTigerFightBetChoice : Enumeration
    {
        public const string DragonValue = "dragon";
        public const string TigerValue = "tiger";
        public const string TieValue = "tie";
        public const string RedDragonValue = "redDragon";
        public const string BlackDragonValue = "blackDragon";
        public const string RedTigerValue = "redTiger";
        public const string BlackTigerValue = "blackTiger";

        public static readonly DragonTigerFightBetChoice Dragon = new DragonTigerFightBetChoice(DragonValue, "Dragon");
        public static readonly DragonTigerFightBetChoice Tiger = new DragonTigerFightBetChoice(TigerValue, "Tiger");
        public static readonly DragonTigerFightBetChoice Tie = new DragonTigerFightBetChoice(TieValue, "Tie");
        public static readonly DragonTigerFightBetChoice RedDragon = new DragonTigerFightBetChoice(RedDragonValue, "Red Dragon");
        public static readonly DragonTigerFightBetChoice BlackDragon = new DragonTigerFightBetChoice(BlackDragonValue, "Black Dragon");
        public static readonly DragonTigerFightBetChoice RedTiger = new DragonTigerFightBetChoice(RedTigerValue, "Red Tiger");
        public static readonly DragonTigerFightBetChoice BlackTiger = new DragonTigerFightBetChoice(BlackTigerValue, "Black Tiger");

        public DragonTigerFightBetChoice()
        { }

        public DragonTigerFightBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}