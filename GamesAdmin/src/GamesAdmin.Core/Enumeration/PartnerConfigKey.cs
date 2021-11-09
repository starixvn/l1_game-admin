namespace GamesAdmin.Core.Enumeration
{
    public class PartnerConfigKey : Enumeration
    {
        public static readonly PartnerConfigKey Name = new PartnerConfigKey("names", "Names");

        public PartnerConfigKey()
        {
        }

        public PartnerConfigKey(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}