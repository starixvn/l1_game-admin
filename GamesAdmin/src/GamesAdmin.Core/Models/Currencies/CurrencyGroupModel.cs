using System.Collections.Generic;

namespace GamesAdmin.Core.Models.Currencies
{
    public class CurrencyGroupModel
    {
        public CurrencyGroupModel(string id, string name, bool enabled, string partnerId, bool fractionalOddsFormat, IEnumerable<CurrencyModel> currencies)
        {
            Id = id;
            Name = name;
            Enabled = enabled;
            PartnerId = partnerId;
            FractionalOddsFormat = fractionalOddsFormat;
            Currencies = currencies;
        }

        public CurrencyGroupModel(string id, string name, bool enabled, string partnerId, bool fractionalOddsFormat)
        {
            Id = id;
            Name = name;
            Enabled = enabled;
            PartnerId = partnerId;
            FractionalOddsFormat = fractionalOddsFormat;
            Currencies = new List<CurrencyModel>();
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public string PartnerId { get; }

        public bool FractionalOddsFormat { get; }

        public IEnumerable<CurrencyModel> Currencies { get; }
    }
}