namespace GamesAdmin.Core.Models.Currencies
{
    public class CurrencyModel
    {
        public CurrencyModel(string id, string name, string code, double multiplier, bool enabled, bool isTest, string partnerId, string groupId)
        {
            Id = id;
            Name = name;
            Code = code;
            Multiplier = multiplier;
            Enabled = enabled;
            IsTest = isTest;
            PartnerId = partnerId;
            GroupId = groupId;
        }

        public string Id { get; }

        public string Name { get; }

        public string Code { get; }

        public double Multiplier { get; }

        public bool Enabled { get; }

        public bool IsTest { get; }

        public string PartnerId { get; }

        public string GroupId { get; }

        public string DisplayText => $"{Name} ({Code})";
    }
}