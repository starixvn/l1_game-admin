using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Extensions
{
    public interface IPartnerPlayerIdConverter
    {
        object ConvertPlayerId(string operatorId, string playerId);
    }

    public class PartnerPlayerIdConverter : IPartnerPlayerIdConverter
    {
        private readonly Dictionary<string, Type> operatorPlayerIdTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "ONEworks1", typeof(int) },
            { "SBO", typeof(int) },
            { "PFF", typeof(string) },
            { "ONEworksVGCash", typeof(string) },
            { "ONEworks_VG", typeof(string) },
            { "ONEworksSeal888", typeof(string) },
            { "SabaPlay", typeof(string) },
        };

        public object ConvertPlayerId(string operatorId, string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                throw new ArgumentException("PlayerId cannot be null or empty.");
            }

            if (operatorPlayerIdTypes.TryGetValue(operatorId, out var playerIdType) && playerIdType == typeof(int))
            {
                if (int.TryParse(playerId, out int parsedId))
                {
                    return parsedId;
                }
                else
                {
                    throw new FormatException($"PlayerId '{playerId}' is not a valid integer for operator '{operatorId}'.");
                }
            }

            return playerId;
        }
    }
}
