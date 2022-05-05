using System.Collections.Generic;

namespace GamesAdmin.Core.Enumeration
{
    public class Language : Enumeration
    {
        public const string VN = "vi-VN";
        public const string EN = "en-US";
        public const string CN = "zh-CN";
        public const string TW = "zh-TW";
        public const string TH = "th-TH";
        public const string ID = "id-ID";

        public const string DefaultLanguageValue = VN;
        public static readonly Language DefaultLanguage = new Language(DefaultLanguageValue, "VN");

        public static Dictionary<string, string> SupportedLanguages = new Dictionary<string, string>
        {
            [VN] = "Message (VN)",
            [EN] = "Message (EN)",
            [CN] = "Message (CN)",
            [TW] = "Message (TW)",
            [TH] = "Message (TH)",
            [ID] = "Message (ID)",
        };

        public static Dictionary<string, string> LanguageMapper = new Dictionary<string, string>
        {
            [VN] = "VNMessage",
            [EN] = "ENMessage",
            [CN] = "CNMessage",
            [TW] = "TWMessage",
            [TH] = "THMessage",
            [ID] = "IDMessage",
        };

        public Language()
        {
        }

        public Language(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}