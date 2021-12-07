namespace GamesAdmin.Core.Enumeration
{
    public class GlobalConfigType : Enumeration
    {
        public static readonly GlobalConfigType SiteExclusion = new GlobalConfigType("game_site_exclusions", "Site Exclusion");
        public static readonly GlobalConfigType SuperPlayers = new GlobalConfigType("super_players", "Super Players");
        public static readonly GlobalConfigType ExchangeCodes = new GlobalConfigType("exchange_codes", "Exchange Codes");

        public GlobalConfigType()
        {
        }

        public GlobalConfigType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}