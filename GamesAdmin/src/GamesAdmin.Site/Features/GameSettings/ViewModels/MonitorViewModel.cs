using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MonitorViewModel
    {
        public MonitorViewModel(IList<MonitorItemViewModel> gameList, AdminMonitoringModel adminMonitoringModel)
        {
            GameList = gameList;
            JwtAdminUserToken = adminMonitoringModel.Token;
            AdminMonitoringUrl = adminMonitoringModel.Url;
        }

        public IList<MonitorItemViewModel> GameList { get; }

        public string AdminMonitoringUrl { get; }

        public string JwtAdminUserToken { get; }
    }

    public class MonitorItemViewModel
    {
        private static readonly string[] disableHistory = new string[]
        {
        };

        private static readonly string[] disableMonitoring = new[] {
            Game.LuckyWheelValue,
            Game.KenoProVietValue,
            Game.BlackJackValue
        };

        public string Name { get; set; }

        public string Id { get; set; }

        public string DisplayName => Enumeration.Contains<Game>(Name)
                                        ? Enumeration.FromValue<Game>(Name).DisplayName
                                        : Name;

        public GameType Type { get; set; }

        public bool EnabledMonitoring => !disableMonitoring.Contains(Name);

        public bool EnabledHistory => !disableHistory.Contains(Name);

        public string GameHistoryUrl => string.IsNullOrWhiteSpace(DisplayName)
            ? Name.Replace(" ", string.Empty).Trim().ToLowerInvariant()
            : DisplayName.Replace(" ", string.Empty).Trim().Replace("'", string.Empty).ToLowerInvariant();
    }
}