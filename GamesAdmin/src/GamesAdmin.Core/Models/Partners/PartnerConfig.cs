using System.Collections.Generic;

namespace GamesAdmin.Core.Models.Partners
{
    public class PartnerConfig
    {
        public PartnerConfig(string partnerId, string type, Dictionary<string, IList<string>> value)
        {
            PartnerId = partnerId;
            Type = type;
            Value = value;
        }

        public string PartnerId { get; }

        public string Type { get; }

        public Dictionary<string, IList<string>> Value { get; }
    }
}