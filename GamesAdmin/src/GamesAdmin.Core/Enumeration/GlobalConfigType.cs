namespace GamesAdmin.Core.Enumeration
{
    public class GlobalConfigType : Enumeration
    {
        public static readonly GlobalConfigType SiteExclusion = new GlobalConfigType("game_site_exclusions", "Site Exclusion");
        public static readonly GlobalConfigType SuperPlayers = new GlobalConfigType("super_players", "Super Players");

        public GlobalConfigType()
        {
        }

        public GlobalConfigType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}