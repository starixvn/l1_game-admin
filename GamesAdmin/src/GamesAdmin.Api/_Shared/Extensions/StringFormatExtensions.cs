namespace GamesAdmin.Api._Shared.Extensions
{
    public static class StringFormatExtensions
    {
        public static string FormatPlayerId(string siteId, string operatorId)
        {
            if (string.IsNullOrEmpty(siteId))
            {
                return operatorId;
            }
            return $"{siteId}~{operatorId}";
        }
    }
}
