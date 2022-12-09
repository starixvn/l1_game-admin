namespace GamesAdmin.Core.Enumeration
{
    public class SlotGame : Enumeration
    {
        public static readonly SlotGame NightClub = new SlotGame("nightclub", "Night Club", GameId.NightClub, "night_club");
        public static readonly SlotGame TreasureIsland = new SlotGame("treasureisland", "Treasure Island", GameId.TreasureIsland, "treasue_island");
        public static readonly SlotGame WayToGoal = new SlotGame("waytogoal", "Way To Goal", GameId.WayToGoal, "way_to_goal");
        public static readonly SlotGame FortuneCat = new SlotGame("fortunecat", "Fortune Cat", GameId.FortuneCat, "fortune_cat");

        public SlotGame()
        {
        }

        public SlotGame(string value, string displayName, GameId id, string serviceName)
            : base(value, displayName)
        {
            GameId = id;
            ServiceName = serviceName;
        }

        public GameId GameId { get; }

        public string ServiceName { get; }

        public static SlotGame FromGameId(GameId id) => Parse<SlotGame, GameId>(id, "game id", item => item.GameId == id);

        public static SlotGame FromGameName(string gameName) => Parse<SlotGame, string>(gameName, "game value", item => item.Value == gameName);

        public static bool IsSlotGame(string gameName) => Contains<SlotGame>(gameName);
    }
}