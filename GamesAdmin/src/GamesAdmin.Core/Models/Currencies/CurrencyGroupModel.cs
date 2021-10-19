using System.Collections.Generic;

namespace GamesAdmin.Core.Models.Currencies
{
    public class CurrencyGroupModel
    {
        public CurrencyGroupModel(string id, string name, bool enabled, string partnerId)
        {
            Id = id;
            Name = name;
            Enabled = enabled;
            PartnerId = partnerId;
        }

        public CurrencyGroupModel(string id, string name, bool enabled, string partnerId, IEnumerable<CurrencyModel> currencies) : this(id, name, enabled, partnerId)
        {
            Currencies = currencies;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public string PartnerId { get; }

        public IEnumerable<CurrencyModel> Currencies { get; }
    }
}