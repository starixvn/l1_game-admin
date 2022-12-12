namespace GamesAdmin.Core.Enumeration
{
    public class SlotGame : Enumeration
    {
        public static readonly SlotGame FourGuardians = new SlotGame("Four Guardians", GameId.FourGuardians);
        public static readonly SlotGame NightClub = new SlotGame( "Night Club", GameId.NightClub);
        public static readonly SlotGame HeroesSlot = new SlotGame( "Heroes Slot", GameId.HeroesSlot);
        public static readonly SlotGame WayToGoal = new SlotGame("Way To Goal", GameId.WayToGoal);
        public static readonly SlotGame FortuneCat = new SlotGame("Fortune Cat", GameId.FortuneCat);

        public SlotGame()
        {
        }

        public SlotGame(string displayName, GameId id)
        {
            GameId = id;
            DisplayName = displayName;
            ServiceName = displayName.Replace(" ", "_").ToLower();
            Value = ServiceName.Replace("_", string.Empty);
        }

        public GameId GameId { get; }

        public string ServiceName { get; }

        public static SlotGame FromGameId(GameId id) => Parse<SlotGame, GameId>(id, "game id", item => item.GameId == id);

        public static SlotGame FromGameName(string gameName) => Parse<SlotGame, string>(gameName, "game value", item => item.Value == gameName);

        public static bool IsSlotGame(string gameName) => Contains<SlotGame>(gameName);
    }
}