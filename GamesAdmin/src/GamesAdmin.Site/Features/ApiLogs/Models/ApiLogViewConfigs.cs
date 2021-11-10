using System;

namespace GamesAdmin.Site.Features.ApiLogs.Models
{
    public class ApiLogViewConfigs
    {
        public ApiLogViewConfigs(string displayName, string path)
        {
            DisplayName = displayName;
            Path = path;
        }

        public ApiLogViewConfigs(string displayName, string path, Func<string, string> formatter)
        {
            DisplayName = displayName;
            Path = path;
            Format = formatter;
        }

        public string DisplayName { get; set; }

        public string Path { get; set; }

        public Func<string, string> Format { get; set; } = input => input;
    }
}