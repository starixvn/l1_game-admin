using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Site.Features.ApiLogs.Models;

namespace GamesAdmin.Site.Features.ApiLogs.ViewModels
{
    public class ApiLogListViewModel
    {
        public IEnumerable<dynamic> Logs { get; set; }

        public IEnumerable<ApiLogViewConfigs> Configs { get; set; }

        public static TValue GetValue<TValue>(dynamic data, string path)
        {
            const char separator = '.';
            var result = data;

            foreach (var prop in path.Split(separator))
            {
                result = result[prop];
            }

            return (TValue)result;
        }

        public IEnumerable<string> Headers() => Configs.Select(config => config.DisplayName);

        public IEnumerable<string> Columns(dynamic data)
            => Configs.Select(config => (string)config.Format(GetValue<string>(data, config.Path)));
    }
}