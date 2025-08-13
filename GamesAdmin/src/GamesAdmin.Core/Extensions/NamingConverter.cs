namespace GamesAdmin.Core.Extensions
{
    public static class NamingConverter
    {
        private const string roundSuffix = "rounds";

        private const string betSuffix = "bets";

        private const string betLogSuffix = "betlogs";

        private const string multipliersSuffix = "multipliers";

        private static string FormatName(string name, string character) => name.Replace(" ", character).ToLowerInvariant();

        private static string CollectionName(string gameName, string suffix) => $"{gameName}_{suffix}";

        public static string GameConfigName(string displayName) => FormatName(displayName, string.Empty);

        public static string ServiceName(string displayName, string separator) => FormatName(displayName, separator ?? "-");

        public static string RoundCollection(string displayName) => CollectionName(GameConfigName(displayName), roundSuffix);

        public static string BetCollection(string displayName) => CollectionName(GameConfigName(displayName), betSuffix);

        public static string BetLogCollection(string displayName) => CollectionName(GameConfigName(displayName), betLogSuffix);

        public static string MultipliersCollection(string displayName) => CollectionName(GameConfigName(displayName), multipliersSuffix);
    }
}