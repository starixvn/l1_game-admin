namespace GamesAdmin.Core.Enumeration
{
    public class PartnerConfigType : Enumeration
    {
        public static readonly PartnerConfigType SiteExclusion = new PartnerConfigType("game_site_exclusions", "Site Exclusion");

        public PartnerConfigType()
        {
        }

        public PartnerConfigType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}