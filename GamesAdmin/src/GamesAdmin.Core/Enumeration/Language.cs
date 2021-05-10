namespace GamesAdmin.Core.Enumeration
{
    public class Language : Enumeration
    {
        public const string VnValue = "vi-VN";
        public const string EnValue = "en-US";

        public static readonly Language Vn = new Language(VnValue, "VN");
        public static readonly Language En = new Language(EnValue, "EN");

        public Language()
        {
        }

        public Language(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}