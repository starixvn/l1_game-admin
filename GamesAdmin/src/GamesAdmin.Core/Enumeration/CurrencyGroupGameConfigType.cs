namespace GamesAdmin.Core.Enumeration
{
    public class CurrencyGroupGameConfigType : Enumeration
    {
        public static readonly CurrencyGroupGameConfigType GuaranteedFeature = new CurrencyGroupGameConfigType("guaranteed_feature", "Guaranteed Feature");

        public CurrencyGroupGameConfigType()
        { }

        public CurrencyGroupGameConfigType(string value, string displayName)
            : base(value, displayName)
        { }
    }
}