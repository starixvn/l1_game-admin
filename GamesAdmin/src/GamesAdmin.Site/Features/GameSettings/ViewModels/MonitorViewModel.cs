using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MonitorViewModel
    {
        public MonitorViewModel(IList<MonitorItemViewModel> gameList)
        {
            GameList = gameList;
        }

        public IList<MonitorItemViewModel> GameList { get; }
    }

    public class MonitorItemViewModel
    {

        private static string[] disableHistory = new string[]
        {
        };

        private static string[] disableMonitoring = new string[]
        {
            Game.LuckyWheelValue,
            Game.KenoProVietValue,
            Game.BlackJackValue
        };

        public MonitorItemViewModel()
        {
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public GameType Type { get; set; }

        public bool EnabledMonitoring => !disableMonitoring.Contains(Name);

        public bool EnabledHistory => !disableHistory.Contains(Name);

        public string GameHistoryUrl => string.IsNullOrWhiteSpace(DisplayName) 
            ? Name.Replace(" ", string.Empty).Trim().ToLowerInvariant()
            : DisplayName.Replace(" ", string.Empty).Trim().ToLowerInvariant();
    }
}
