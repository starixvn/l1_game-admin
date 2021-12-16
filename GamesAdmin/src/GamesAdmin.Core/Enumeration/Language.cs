using System.Collections.Generic;

namespace GamesAdmin.Core.Enumeration
{
    public class Language : Enumeration
    {
        public static Dictionary<string, string> SupportedLanguages = new Dictionary<string, string>
        {
            ["vi-VN"] = "Message (VN)",
            ["en-US"] = "Message (EN)",
            ["zh-CN"] = "Message (CN)",
            ["zh-TW"] = "Message (TW)",
            ["th-TH"] = "Message (TH)",
            ["id-ID"] = "Message (ID)",
        };

        public static Dictionary<string, string> LanguageMapper = new Dictionary<string, string>
        {
            ["vi-VN"] = "VNMessage",
            ["en-US"] = "ENMessage",
            ["zh-CN"] = "CNMessage",
            ["zh-TW"] = "TWMessage",
            ["th-TH"] = "THMessage",
            ["id-ID"] = "IDMessage",
        };

        public const string DefaultLanguageValue = "vi-VN";
        public static readonly Language DefaultLanguage = new Language(DefaultLanguageValue, "VN");

        public Language()
        {
        }

        public Language(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}