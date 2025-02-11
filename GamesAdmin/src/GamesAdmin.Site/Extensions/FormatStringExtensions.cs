using System.Globalization;
using System.Text.RegularExpressions;

namespace GamesAdmin.Site.Extensions
{
    public static class FormatStringExtensions
    {
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string ToTitleCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(s);
        }

        public static string ToTitleCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            var result = Regex.Replace(s, "[A-Z]", " $0");
            return result.UppercaseFirst();
        }
    }
}