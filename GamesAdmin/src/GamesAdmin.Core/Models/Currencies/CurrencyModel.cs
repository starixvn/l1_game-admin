namespace GamesAdmin.Core.Models.Currencies
{
    public class CurrencyModel
    {
        public CurrencyModel(string id, string name, string code, double rate, bool enabled, bool isTest, string partnerId, string currencyGroupId)
        {
            Id = id;
            Name = name;
            Code = code;
            Rate = rate;
            Enabled = enabled;
            IsTest = isTest;
            PartnerId = partnerId;
            CurrencyGroupId = currencyGroupId;
        }

        public string Id { get; }

        public string Name { get; }

        public string Code { get; }

        public double Rate { get; }

        public bool Enabled { get; }

        public bool IsTest { get; }

        public string PartnerId { get; }

        public string CurrencyGroupId { get; }
    }
}